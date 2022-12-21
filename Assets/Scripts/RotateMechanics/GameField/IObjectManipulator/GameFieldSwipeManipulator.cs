using UnityEngine;

namespace RotateMechanics.GameField.IObjectManipulator
{
    public sealed class GameFieldSwipeManipulator : ObjectManipulatorBase
    {

        public override void OnInputStart(Vector2 position)
        {
            StartPosition = position;
            if (!_mainObject.Selected)
            {
                
            }
        }

        public override void OnInputHold(Vector2 position)
        {
            
        }

        public override void OnInputUp(Vector2 position)
        {
            var normalized = (position - StartPosition).normalized;
            TryMoveMainObject(GameFieldMath.CalculateNewPosition(_mainObject, normalized.x, normalized.y, 1));
        }
    }
}