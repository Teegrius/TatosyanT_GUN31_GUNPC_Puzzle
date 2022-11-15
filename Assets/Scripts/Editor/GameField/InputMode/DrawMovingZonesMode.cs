using System;
using Core;
using SceneObjects;
using UnityEditor;
using UnityEngine;
using GameFieldManager = RotateMechanics.GameField.GameFieldManager;
using Object = UnityEngine.Object;

namespace Editor.GameField.InputMode
{
    public sealed class DrawMovingZonesMode : IEditSceneMode
    {
        private readonly GameFieldManager _gameFieldManager;
        private readonly Action<MovingZone> _onMovingZone;

        public string Name => "Draw Zones";

        public DrawMovingZonesMode(GameFieldManager gameFieldManager, Action<MovingZone> onMovingZone)
        {
            _gameFieldManager = gameFieldManager;
            _onMovingZone = onMovingZone;
        }

        public void OnModeSelected() => Tools.current = Tool.Move;

        public void ProcessInput(Vector2 mousePosition)
        {
            if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseDrag)
            {
                return;
            }

            if (!_gameFieldManager.TryGetMovePoint(mousePosition, out var movePoint))
            {
                return;
            }

            if (_gameFieldManager.TryGetMovingZone(mousePosition, out _))
            {
                return;
            };

            var movingZoneObject = Object.Instantiate(Resources.Load("MovingZone")) as GameObject;
            movingZoneObject.transform.position = movePoint.transform.position;
            _onMovingZone?.Invoke(movingZoneObject.GetComponent<MovingZone>());
        }
    }
}