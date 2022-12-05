using RotateMechanics.GameField;
using UnityEngine;

namespace Editor.GameField.InputMode
{
    public sealed class DrawStarsMode : IEditSceneMode
    {
        private readonly GameFieldManager _gameFieldManager;

        public DrawStarsMode(GameFieldManager gameFieldManager) => _gameFieldManager = gameFieldManager;

        public string Name => "Draw Stars";
        public void ProcessInput(Vector2 mousePosition)
        {
            if (Event.current.type != EventType.MouseUp)
            {
                return;
            }

            
        }
    }
}