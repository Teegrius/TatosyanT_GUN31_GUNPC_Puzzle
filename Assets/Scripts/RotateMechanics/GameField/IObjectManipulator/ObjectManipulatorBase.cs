using SceneObjects;
using UnityEngine;

namespace RotateMechanics.GameField.IObjectManipulator
{
    public abstract class ObjectManipulatorBase : IObjectManipulator
    {
        [SerializeField] protected MainObject _mainObject;
        [SerializeField] protected TargetObject _targetObject;
        [SerializeField] protected MovePoint[] _movePoints;
        [SerializeField] protected MovingZone[] _movingZones;
        protected Vector2 StartPosition;
        protected Transform MovePointsTransform;
        private Quaternion _defaultFieldRotation;
        private Vector2 _mainObjectDefaultPosition;

        public abstract void OnInputStart(Vector2 position);

        public abstract void OnInputHold(Vector2 position);

        public abstract void OnInputUp(Vector2 position);

        public void Initialize()
        {
            MovePointsTransform = _movePoints[0].transform.parent;
            _defaultFieldRotation = MovePointsTransform.rotation;
            _mainObjectDefaultPosition = _mainObject.Position;
            AdjustLevelObjectsToMovePoints(_mainObject);
            AdjustLevelObjectsToMovePoints(_targetObject);
        }

        public void Reset()
        {
            MovePointsTransform.rotation = _defaultFieldRotation;
            _mainObject.Transform.position = _mainObjectDefaultPosition;
            AdjustLevelObjectsToMovePoints(_mainObject);
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
        
        protected void TryMoveMainObject(Vector2 newPosition)
        {
            if (TryGetMovePoint(newPosition, out var movePoint) && IsWithinMoveZone(movePoint))
            {
                _mainObject.Transform.parent = movePoint.Transform;
                _mainObject.Transform.localPosition = Vector3.zero;
            }
        }

        private bool IsWithinMoveZone(MovePoint movePoint)
        {
            var initialZone = GetMovingZone(_mainObject.Transform.position);
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
    }
}