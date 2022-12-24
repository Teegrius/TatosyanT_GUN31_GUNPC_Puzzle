using DefaultNamespace;
using UnityEngine;

namespace RotateMechanics.GameField.ObjectManipulators
{
    public sealed class GameFieldDragNDropManipulator : ObjectManipulatorBase
    {
        private Vector2 _mainObjectStartPosition;

        public override void OnInputStart(Vector2 position)
        {
            StartPosition = position;
            if (!MainObject.Selected)
            {
                IsRotating = true;
            }
            else
            {
                _mainObjectStartPosition = MainObject.Position;
            }
        }

        public override void OnInputHold(Vector2 position)
        {
            MainObject.Transform.position = _mainObjectStartPosition;
            var deltaInput = (position - StartPosition);
            if (Mathf.Abs(deltaInput.x) > RotateConstants.Half || Mathf.Abs(deltaInput.y) > RotateConstants.Half)
            {
                var inputNormalized = deltaInput.normalized;
                var newPos = GameFieldMath.CalculateNewPosition(MainObject, inputNormalized.x, inputNormalized.y, 1);
                MainObject.Transform.position =
                    newPos;
            }
        }

        public override void OnInputUp(Vector2 position)
        {
            if (IsRotating)
            {
                Rotate(position);
                return;
            }
            MainObject.Transform.position = _mainObjectStartPosition;
            var normalized = (position - StartPosition).normalized;
            TryMoveMainObject(GameFieldMath.CalculateNewPosition(MainObject, normalized.x, normalized.y, 1));
        }
    }
}