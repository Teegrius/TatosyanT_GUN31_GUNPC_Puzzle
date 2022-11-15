using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.InputSource
{
    public abstract class VectorInputSource : IVectorInputSource
    {
        private Vector2 _firstPressPos;
        private Vector2 _secondPressPos;

        protected abstract bool GetInputDown { get; }
        protected abstract bool GetInputHold { get; }
        protected abstract bool GetInputUp { get; }

        public bool IsDown { get; private set; }
        public bool IsHold { get; private set; }
        public bool IsUp { get; private set; }

        public Vector2 InputPosition { get; private set; }

        protected abstract Vector2 GetInputPosition();

        public void Check()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                IsDown = false;
                IsHold = false;
                IsUp = false;
                return;
            }
            IsDown = GetInputDown;
            IsHold = GetInputHold;
            IsUp = GetInputUp;

            InputPosition = Camera.main.ScreenToWorldPoint(GetInputPosition());
        }
    }
}