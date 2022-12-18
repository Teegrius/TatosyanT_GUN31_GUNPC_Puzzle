using RotateMechanics.GameField;
using UnityEngine;

namespace Editor.GameField.InputMode
{
    public sealed class DrawStarsMode : IEditSceneMode
    {
        private readonly GameFieldManager _gameFieldManager;

        public DrawStarsMode(GameFieldManager gameFieldManager) => _gameFieldManager = gameFieldManager;

        public string Name => "Draw Stars";

        public string Description =>
            "Click on a point to draw a star on it. You can't draw star above other objects (except for gray zone). 3 stars max";

        public void ProcessInput(Vector2 mousePosition)
        {
            if (Event.current.type != EventType.MouseUp)
            {
                return;
            }

            if (!_gameFieldManager.TryGetMovePoint(mousePosition, out var movePoint))
            {
                return;
            }

            if (!_gameFieldManager.TryAddStar(movePoint, out var message))
            {
                Debug.LogError(message);
            }
        }
    }
}