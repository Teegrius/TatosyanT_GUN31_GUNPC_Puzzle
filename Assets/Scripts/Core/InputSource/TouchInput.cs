using UnityEngine;

namespace Core.InputSource
{
    public sealed class TouchInput : VectorInputSource
    {
        protected override bool GetInputDown => TryGetTouch(out var touch) && touch.phase == TouchPhase.Began;
        protected override bool GetInputHold => TryGetTouch(out var touch) && touch.phase == TouchPhase.Moved;
        protected override bool GetInputUp => TryGetTouch(out var touch) && touch.phase == TouchPhase.Ended;

        private bool TryGetTouch(out Touch touch)
        {
            if (Input.touches.Length > 0)
            {
                touch = Input.GetTouch(0);
                return true;
            }

            touch = default;
            return false;
        }

        protected override Vector2 GetInputPosition() => TryGetTouch(out var touch) ? touch.position : default;
    }
}