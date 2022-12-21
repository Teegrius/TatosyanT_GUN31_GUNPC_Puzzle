using DefaultNamespace;
using UnityEngine;

namespace RotateMechanics.GameField.IObjectManipulator
{
    public sealed class GameFieldDragNDropManipulator : ObjectManipulatorBase
    {
        private Vector2 _mainObjectStartPosition;

        public override void OnInputStart(Vector2 position)
        {
            StartPosition = position;
            if (!_mainObject.Selected)
            {
                
            }
            else
            {
                _mainObjectStartPosition = _mainObject.Position;
            }
        }

        public override void OnInputHold(Vector2 position)
        {
            _mainObject.Transform.position = _mainObjectStartPosition;
            var deltaInput = (position - StartPosition);
            if (Mathf.Abs(deltaInput.x) > RotateConstants.Half || Mathf.Abs(deltaInput.y) > RotateConstants.Half)
            {
                var inputNormalized = deltaInput.normalized;
                var newPos = GameFieldMath.CalculateNewPosition(_mainObject, inputNormalized.x, inputNormalized.y, 1);
                _mainObject.Transform.position =
                    newPos;
            }
        }

        public override void OnInputUp(Vector2 position)
        {
            _mainObject.Transform.position = _mainObjectStartPosition;
            var normalized = (position - StartPosition).normalized;
            TryMoveMainObject(GameFieldMath.CalculateNewPosition(_mainObject, normalized.x, normalized.y, 1));
        }
    }
}