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
        private Vector3 startPosition;
        private Vector3 endPosition;

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
                _drawing = false;
                MapZoneToPoints(_drawer.MovingZone, movePoint);
            }
        }

        private void MapZoneToPoints(MovingZone drawerMovingZone, MovePoint endPoint)
        {
            startPosition = _startPoint.transform.position;

            endPosition = endPoint.transform.position;
            if (startPosition.x != endPosition.x &&
                startPosition.y != endPosition.y)
            {
                Debug.LogError("Can't map moving zone to grid. Moving zone should be horizontal or vertical");
                Object.DestroyImmediate(drawerMovingZone.gameObject);
                return;
            }

            if (Mathf.Approximately(startPosition.x ,endPosition.x))
            {
                SetZoneSize(drawerMovingZone, startPosition.y, endPosition.y, out var centerY);
                drawerMovingZone.transform.rotation = Quaternion.Euler(0,0,90);
                drawerMovingZone.transform.position = new Vector2(endPosition.x, centerY);

            }

            if (Mathf.Approximately(startPosition.y,endPosition.y))
            {
                SetZoneSize(drawerMovingZone, startPosition.x, endPosition.x, out var centerX);
                drawerMovingZone.transform.rotation = Quaternion.Euler(0,0,0);
                drawerMovingZone.transform.position = new Vector2(centerX, endPosition.y);
            }
            _onMovingZone?.Invoke(drawerMovingZone);
        }

        private void SetZoneSize(MovingZone drawerMovingZone, float start, float end, out float center)
        {
            center = (start + end) / 2f;
            var length = Mathf.Abs(end - start) + 0.5f;
            drawerMovingZone.GetComponent<SpriteRenderer>().size =
                new Vector2(length, 0.5f);
        }
    }
}