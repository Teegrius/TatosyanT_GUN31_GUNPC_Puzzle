using UnityEngine;

namespace RotateMechanics.GameField.ObjectManipulators
{
    public sealed class GameFieldSwipeManipulator : ObjectManipulatorBase
    {

        public override void OnInputStart(Vector2 position)
        {
            StartPosition = position;
            if (!MainObject.Selected)
            {
                IsRotating = true;
            }
        }

        public override void OnInputHold(Vector2 position)
        {
        }

        public override void OnInputUp(Vector2 position)
        {
            if (IsRotating)
            {
                Rotate(position);
                return;
            }
            var normalized = (position - StartPosition).normalized;
            TryMoveMainObject(GameFieldMath.CalculateNewPosition(MainObject, normalized.x, normalized.y, 1));
        }
    }
}