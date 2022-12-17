using System;
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
        private readonly MovingZonesDrawer _drawer;
        private bool _drawing;
        private MovePoint _startPoint;

        public string Name => "Draw Zones";

        public DrawMovingZonesMode(GameFieldManager gameFieldManager, Action<MovingZone> onMovingZone)
        {
            _gameFieldManager = gameFieldManager;
            _onMovingZone = onMovingZone;
            _drawer = new MovingZonesDrawer();
        }

        public void OnModeSelected() => Tools.current = Tool.Move;

        public void ProcessInput(Vector2 mousePosition)
        {
            if (Event.current.type == EventType.MouseUp)
            {
                ProcessMouseUp(mousePosition);
            }

            if (Event.current.type == EventType.MouseMove)
            {
                ProcessMouseMove(mousePosition);
            }
        }

        private void ProcessMouseMove(Vector2 mousePosition)
        {
            if (_drawing)
            {
                _drawer.DrawTowardsMousePosition(mousePosition);
            }
        }

        private void ProcessMouseUp(Vector2 mousePosition)
        {
            if (!_gameFieldManager.TryGetMovePoint(mousePosition, out var movePoint))
            {
                return;
            }
            if (!_drawing)
            {
                _startPoint = movePoint;
                _drawing = true;
                _drawer.StartDraw(_startPoint.transform.position);
            }
            else
            {
                _drawer.StopDraw();
                MapZoneToPoints(_drawer.MovingZone, movePoint);
            }
            
            
            
        }

        private void MapZoneToPoints(MovingZone drawerMovingZone, MovePoint endPoint)
        {
            if (_startPoint.transform.position.x != endPoint.transform.position.x ||
                _startPoint.transform.position.y != endPoint.transform.position.y)
            {
                Debug.LogError("Can't map moving zone to grid. Moving zone should be horizontal or vertical");
                Object.DestroyImmediate(drawerMovingZone.gameObject);
            }

            if (_startPoint.transform.position.x == endPoint.transform.position.x)
            {
                
            }

            if (_startPoint.transform.position.y == endPoint.transform.position.y)
            {
                
            }
        }
    }
}