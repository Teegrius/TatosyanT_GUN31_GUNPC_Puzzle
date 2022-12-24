using System.Collections.Generic;
using Core.MessageSystem;
using Messages;
using Messages.Input;
using RotateMechanics.GameField.ObjectManipulators;
using SceneObjects;
using UnityEngine;

namespace RotateMechanics.GameField
{
    public sealed partial class GameFieldManager : MonoBehaviour, 
        IGameFieldManager,
        IMessageListener<SetSwipeInput>, 
        IMessageListener<SetDragNDropInput>
    {
        private const int MaxStarsCount = 3;
        private readonly string Swipe = nameof(Swipe);
        private readonly string DragNDrop = nameof(DragNDrop);

        [SerializeField] private int _totalColumns = 5;
        [SerializeField] private int _totalRows = 5;
        [SerializeField] private float _gridSize = 1f;

        [SerializeField] private MainObject _mainObject;
        [SerializeField] private TargetObject _targetObject;
        [SerializeField] private MovePoint[] _movePoints;
        [SerializeField] private MovingZone[] _movingZones;
        [SerializeField] private List<StarObject> _stars;

        private IObjectManipulator _currentObjectManipulator;
        private Dictionary<string, IObjectManipulator> _availableManipulators;

        private void Start() => Initialize();

        public void Initialize()
        {
            var swipeManipulator = new GameFieldSwipeManipulator();
            swipeManipulator.Initialize(_mainObject, _targetObject, _movePoints, _movingZones);
            var dragNDropManipulator = new GameFieldDragNDropManipulator();
            dragNDropManipulator.Initialize(_mainObject, _targetObject, _movePoints, _movingZones);
            _availableManipulators = new Dictionary<string, IObjectManipulator>()
            {
                { Swipe, swipeManipulator},
                { DragNDrop, dragNDropManipulator}
            };
            _currentObjectManipulator = swipeManipulator;
            Subscribe();
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

        private void CheckWinCondition()
        {
            if (_mainObject.IsOnSamePosition(_targetObject))
            {
                Messenger.Send(new LevelCompleted());
                Messenger.Send(new SetInputActiveState {IsActive = false});
            }
        }

        public void OnMessage(LevelRestarted message) => _currentObjectManipulator.Reset();

        public void OnMessage(SetSwipeInput message) => _currentObjectManipulator = _availableManipulators[Swipe];

        public void OnMessage(SetDragNDropInput message) => _currentObjectManipulator = _availableManipulators[DragNDrop];

        public void OnMessage(InputStarted message) => _currentObjectManipulator.OnInputStart(message.Position);

        public void OnMessage(InputHold message) => _currentObjectManipulator.OnInputHold(message.Position);

        public void OnMessage(InputFinished message) => _currentObjectManipulator.OnInputUp(message.Position);
    }
}