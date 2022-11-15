using UnityEngine;

namespace Core.InputSource
{
    public sealed class MouseInput : VectorInputSource
    {
        protected override bool GetInputDown => Input.GetMouseButtonDown(0);
        protected override bool GetInputHold => Input.GetMouseButton(0);
        protected override bool GetInputUp => Input.GetMouseButtonUp(0);

        protected override Vector2 GetInputPosition() => new(Input.mousePosition.x, Input.mousePosition.y);
    }
}