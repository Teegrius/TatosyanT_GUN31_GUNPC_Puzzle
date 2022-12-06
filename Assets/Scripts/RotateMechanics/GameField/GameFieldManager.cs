using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.MessageSystem;
using Messages;
using Messages.Input;
using RSG;
using SceneObjects;
using UnityEngine;

namespace RotateMechanics.GameField
{
    public sealed partial class GameFieldManager : MonoBehaviour, 
        IMessageListener<LevelRestarted>, 
        IMessageListener<SetSwipeInput>, 
        IMessageListener<SetDragNDropInput>,
        IMessageListener<InputStarted>,
        IMessageListener<InputHold>,
        IMessageListener<InputFinished>
        
    {
        #region Constants

        private const int FieldRotationAngle = 90;
        private const int MaxStarsCount = 3;
        
        #endregion

        #region Serialize Fields

        [SerializeField] private int _totalColumns = 5;
        [SerializeField] private int _totalRows = 5;
        [SerializeField] private float _gridSize = 1f;

        [SerializeField] private MainObject _mainObject;
        [SerializeField] private TargetObject _targetObject;
        [SerializeField] private MovePoint[] _movePoints;
        [SerializeField] private MovingZone[] _movingZones;
        [SerializeField] private List<StarObject> _stars;

        #endregion

        #region Private NonSerialized Fields

        private Quaternion _defaultFieldRotation;
        private Vector2 _mainObjectDefaultPosition;

        private Transform _movePointsTransform;
        private bool _isRotating;

        private Dictionary<Vector2, MovePoint> _movePositions;
        
        #endregion

        #region State Machine

        private IState _stateMachine;
        //states
        private readonly string Idle = nameof(Idle);
        private readonly string SwipeState = nameof(SwipeState);
        private readonly string DragNDropState = nameof(DragNDropState);
        private readonly string RotateLevelState = nameof(RotateLevelState);

        //events
        private readonly string Swipe = nameof(Swipe);
        private readonly string DragNDrop = nameof(DragNDrop);
        private readonly string InputStarted = nameof(InputStarted);
        private readonly string InputHold = nameof(InputHold);
        private readonly string InputFinished = nameof(InputFinished);

        private Vector2 _startPosition;
        private Vector2 _mainObjectStartPosition;

        private sealed class InputEventArgs : EventArgs
        {
            public readonly Vector2 Position;

            public InputEventArgs(Vector2 position) => Position = position;
        }

        #endregion

        #region Event Methods

        private void Start()
        {
            _movePositions = _movePoints.ToDictionary(point => point.Position, point => point);
            _movePointsTransform = _movePoints[0].transform.parent;
            _defaultFieldRotation = _movePointsTransform.rotation;
            _mainObjectDefaultPosition = _mainObject.Position;

            _mainObject.Transform.parent = _movePositions[_mainObject.Position].Transform;
            _targetObject.Transform.parent = _movePositions[_targetObject.Position].Transform;
            Subscribe();
            CreateStateMachine();
        }

        private void CreateStateMachine()
        {
            _stateMachine = new StateMachineBuilder()
                .State(SwipeState)
                .Event<InputEventArgs>(InputStarted, (_, args) =>
                {
                    _startPosition = args.Position;
                    if (!_mainObject.Selected)
                    {
                        _stateMachine.ChangeState(RotateLevelState);
                    }
                })
                .Event<InputEventArgs>(InputFinished, (_, args) =>
                {
                    var normalized = (args.Position - _startPosition).normalized;
                    TryMoveMainObject(GameFieldMath.CalculateNewPosition(_mainObject, normalized.x, normalized.y, _gridSize));
                })
                .Event(DragNDrop, _ => _stateMachine.ChangeState(DragNDropState))
                .End()
                .State(DragNDropState)
                .Event(Swipe, _ => _stateMachine.ChangeState(SwipeState))
                .Event<InputEventArgs>(InputStarted, (_, args) =>
                {
                    _startPosition = args.Position;
                    if (!_mainObject.Selected)
                    {
                        _stateMachine.ChangeState(RotateLevelState);
                    }
                    else
                    {
                        _mainObjectStartPosition = _mainObject.Position;
                    }
                })
                .Event<InputEventArgs>(InputHold, (_, args) =>
                {
                    _mainObject.Transform.position = _mainObjectStartPosition;
                    var deltaInput = (args.Position - _startPosition);
                    if (Mathf.Abs(deltaInput.x) > 0.5f || Mathf.Abs(deltaInput.y) > 0.5f)
                    {
                        var inputNormalized = deltaInput.normalized;
                        var newPos = GameFieldMath.CalculateNewPosition(_mainObject, inputNormalized.x, inputNormalized.y, _gridSize);
                        _mainObject.Transform.position =
                            newPos;
                    }
                })
                .Event<InputEventArgs>(InputFinished, (_, args) =>
                {
                    _mainObject.Transform.position = _mainObjectStartPosition;
                    var normalized = (args.Position - _startPosition).normalized;
                    TryMoveMainObject(GameFieldMath.CalculateNewPosition(_mainObject, normalized.x, normalized.y, _gridSize));
                })
                .End()
                .State(RotateLevelState)
                .Enter(_ => _isRotating = true)
                .Event<InputEventArgs>(InputFinished, (_, args) =>
                {
                    Messenger.Send(new SetInputActiveState {IsActive = false});
                    var delta = args.Position - _startPosition;
                    if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        StartCoroutine(delta.x > 0
                            ? RotateRoutine(FieldRotationAngle)
                            : RotateRoutine(-FieldRotationAngle));
                        return;
                    }
                    StartCoroutine(delta.y > 0
                        ? RotateRoutine(-FieldRotationAngle)
                        : RotateRoutine(FieldRotationAngle));
                })
                .Update((_, _) =>
                {
                    if (!_isRotating)
                    {
                        _stateMachine.ChangeState(SwipeState);
                    }
                })
                .Exit(_ => Messenger.Send(new SetInputActiveState {IsActive = true}))
                .End()
                .Build();
            _stateMachine.ChangeState(SwipeState);
        }

        private void Subscribe()
        {
            Messenger.Subscribe<LevelRestarted>(this);
            Messenger.Subscribe<SetSwipeInput>(this);
            Messenger.Subscribe<SetDragNDropInput>(this);
            Messenger.Subscribe<InputStarted>(this);
            Messenger.Subscribe<InputHold>(this);
            Messenger.Subscribe<InputFinished>(this);
        }

        private void Update() => _stateMachine.Update(Time.deltaTime);

        private void OnDestroy() => Unsubscribe();

        private void Unsubscribe()
        {
            Messenger.Unsubscribe<LevelRestarted>(this);
            Messenger.Unsubscribe<SetSwipeInput>(this);
            Messenger.Unsubscribe<SetDragNDropInput>(this);
            Messenger.Unsubscribe<InputStarted>(this);
            Messenger.Unsubscribe<InputHold>(this);
            Messenger.Unsubscribe<InputFinished>(this);
        }

        #endregion

        #region Mechanics Methods

        private void TryMoveMainObject(Vector2 newPosition)
        {
            if (_movePositions.TryGetValue(newPosition, out var movePoint) && IsWithinMoveZone(newPosition))
            {
                _mainObject.Transform.position = newPosition;
                _mainObject.Transform.parent = movePoint.Transform;
                CheckWinCondition();
            }
        }

        private void CheckWinCondition()
        {
            if (_mainObject.Position == _targetObject.Position)
            {
                Messenger.Send(new LevelCompleted());
                Messenger.Send(new SetInputActiveState {IsActive = true});
            }
        }

        private bool IsWithinMoveZone(Vector2 newPosition)
        {
            for (var i = 0; i < _movingZones.Length; i++)
            {
                var movingZone = _movingZones[i];
                if (movingZone.Position.x == newPosition.x && movingZone.Position.y == newPosition.y)
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerator RotateRoutine(int angle)
        {
            _isRotating = true;
            var oldRotation = _movePointsTransform.transform.rotation;
            var newRotation = oldRotation * Quaternion.Euler(0, 0, angle);
            float t = 0;
            while (t <= 1)
            {
                _movePointsTransform.transform.rotation = Quaternion.Lerp(oldRotation, newRotation, t);
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _movePointsTransform.transform.rotation = newRotation;
            _isRotating = false;
        }

        private void ResetLevel()
        {
            StopCoroutine(RotateRoutine(0));
            _isRotating = false;
            _movePointsTransform.rotation = _defaultFieldRotation;
            _mainObject.Transform.position = _mainObjectDefaultPosition;
            _mainObject.Transform.parent = _movePositions[_mainObject.Position].Transform;
            _targetObject.Transform.parent = _movePositions[_targetObject.Position].Transform;
        }

        #endregion

        #region IMessageListener implementations

        public void OnMessage(LevelRestarted message) => ResetLevel();

        public void OnMessage(SetSwipeInput message) => _stateMachine.TriggerEvent(Swipe);

        public void OnMessage(SetDragNDropInput message) => _stateMachine.TriggerEvent(DragNDrop);

        public void OnMessage(InputStarted message) => _stateMachine.TriggerEvent(InputStarted, new InputEventArgs(message.Position));

        public void OnMessage(InputHold message) => _stateMachine.TriggerEvent(InputHold, new InputEventArgs(message.Position));

        public void OnMessage(InputFinished message) => _stateMachine.TriggerEvent(InputFinished, new InputEventArgs(message.Position));

        #endregion
    }
}