using RotateMechanics.GameField;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.GameField.InputMode
{
    public sealed class EraseStarMode : IEditSceneMode
    {
        private readonly GameFieldManager _gameFieldManager;

        public EraseStarMode(GameFieldManager gameFieldManager) => _gameFieldManager = gameFieldManager;

        public string Name => "Erase Stars";

        public string Description => "Click on a star to erase it";

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

            _gameFieldManager.TryRemoveStar(movePoint);
            EditorUtility.SetDirty(_gameFieldManager);
        }
    }
}