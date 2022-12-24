using Core.MessageSystem;
using Core.Tweener;
using DefaultNamespace;
using Messages;
using SceneObjects;
using UnityEngine;

namespace RotateMechanics.GameField.ObjectManipulators
{
    public abstract class ObjectManipulatorBase : IObjectManipulator
    {
        protected Vector2 StartPosition;
        protected bool IsRotating;
        protected MainObject MainObject;
        
        private TargetObject _targetObject;
        private MovePoint[] _movePoints;
        private MovingZone[] _movingZones;
        private Transform _movePointsTransform;
        private Quaternion _defaultFieldRotation;
        private Vector2 _mainObjectDefaultPosition;

        private ITweener _rotateTweener;

        public abstract void OnInputStart(Vector2 position);

        public abstract void OnInputHold(Vector2 position);

        public abstract void OnInputUp(Vector2 position);

        public void Initialize(MainObject mainObject, TargetObject targetObject, MovePoint[] movePoints, MovingZone[] movingZones)
        {
            MainObject = mainObject;
            _targetObject = targetObject;
            _movePoints = movePoints;
            _movingZones = movingZones;
            InitializeFields();

            void InitializeFields()
            {
                _movePointsTransform = _movePoints[0].transform.parent;
                _defaultFieldRotation = _movePointsTransform.rotation;
                _mainObjectDefaultPosition = MainObject.Position;
                AdjustLevelObjectsToMovePoints(MainObject);
                AdjustLevelObjectsToMovePoints(_targetObject);
            }
        }

        public void Reset()
        {
            _movePointsTransform.rotation = _defaultFieldRotation;
            MainObject.Transform.position = _mainObjectDefaultPosition;
            AdjustLevelObjectsToMovePoints(MainObject);
            AdjustLevelObjectsToMovePoints(_targetObject);
        }

        private void AdjustLevelObjectsToMovePoints(SceneObjectAbstract sceneObjectAbstract)
        {
            if (TryGetMovePoint(sceneObjectAbstract.Transform.position, out var movePoint))
            {
                sceneObjectAbstract.Transform.parent = movePoint.Transform;
                sceneObjectAbstract.Transform.localPosition = Vector3.zero;
            }
        }

        private bool TryGetMovePoint(Vector2 point, out MovePoint movePoint)
        {
            for (var i = 0; i < _movePoints.Length; i++)
            {
                if (_movePoints[i].IsOnSamePosition(point))
                {
                    movePoint = _movePoints[i];
                    return true;
                }
            }

            movePoint = default;
            return false;
        }

        protected void TryMoveMainObject(Vector2 newPosition, bool animate = false)
        {
            if (TryGetMovePoint(newPosition, out var movePoint) && IsWithinMoveZone(movePoint))
            {
                if (!animate)
                {
                    MainObject.Transform.parent = movePoint.Transform;
                    MainObject.Transform.localPosition = Vector3.zero;
                    return;
                }
            }
        }

        private bool IsWithinMoveZone(MovePoint movePoint)
        {
            var initialZone = GetMovingZone(MainObject.Transform.position);
            var newZone = GetMovingZone(movePoint.Position);
            if (initialZone == null || newZone == null)
            {
                return false;
            }
            return initialZone == newZone || newZone.HasIntersection(initialZone);

            MovingZone GetMovingZone(Vector2 position)
            {
                for (int i = 0; i < _movingZones.Length; i++)
                {
                    if (_movingZones[i].IsOnSamePosition(position))
                    {
                        return _movingZones[i];
                    }
                }

                return null;
            }
        }

        protected void Rotate(Vector2 position)
        {
            var delta = position - StartPosition;
            float angle = 0;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                angle = delta.x > RotateConstants.Zero ? -RotateConstants.FieldRotationAngle : RotateConstants.FieldRotationAngle;
            }
            else
            {
                angle = delta.y > RotateConstants.Zero ? RotateConstants.FieldRotationAngle : -RotateConstants.FieldRotationAngle;
            }
            _rotateTweener = TweenFactory.RotateAround(_movePointsTransform.gameObject, angle, Vector3.forward)
                .OnStart(() =>
                {
                    Messenger.Send(new SetInputActiveState { IsActive = false });
                    MainObject.DisableTrail();
                })
                .OnFinish(() =>
                {
                    Messenger.Send(new SetInputActiveState { IsActive = true });
                    IsRotating = false;
                    MainObject.EnableTrail();
                });
            _rotateTweener.Stop();
        }
    }
}