using System;
using SceneObjects;
using UnityEngine;
using GameFieldManager = RotateMechanics.GameField.GameFieldManager;

namespace Editor.GameField.InputMode
{
    public sealed class EraseMovingZonesMode : IEditSceneMode
    {
        private readonly GameFieldManager _gameFieldManager;
        private readonly Action<MovingZone> _onMovingZone;

        public EraseMovingZonesMode(GameFieldManager gameFieldManager, Action<MovingZone> onMovingZone)
        {
            _gameFieldManager = gameFieldManager;
            _onMovingZone = onMovingZone;
        }

        public string Name => "Erase zones";

        public void ProcessInput(Vector2 mousePosition)
        {
            if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseDrag)
            {
                return;
            }

            if (_gameFieldManager.TryGetMovingZone(mousePosition, out var movingZone))
            {
                _onMovingZone?.Invoke(movingZone);
            }
        }
    }
}