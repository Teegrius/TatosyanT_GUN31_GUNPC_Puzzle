using SceneObjects;
using UnityEditor;
using UnityEngine;

namespace Editor.GameField.InputMode
{
    public sealed class MovingZonesDrawer
    {
        private SpriteRenderer _renderer;
        private bool _startedDrawing;
        private Vector2 _startPosition;

        public MovingZone MovingZone => _renderer.GetComponent<MovingZone>();

        public void StartDraw(Vector2 position)
        {
            _startedDrawing = true;
            var movingZoneObject = PrefabUtility.InstantiatePrefab(Resources.Load("MovingZone")) as GameObject;
            _renderer = movingZoneObject.GetComponent<SpriteRenderer>();
            movingZoneObject.transform.position = position;
            _startPosition = position;
        }

        public void StopDraw() => _startedDrawing = false;

        public void DrawTowardsMousePosition(Vector2 position)
        {
            if (!_startedDrawing)
            {
                return;
            }
            float angleRad = Mathf.Atan2(position.y - _renderer.transform.position.y, position.x - _renderer.transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            _renderer.transform.rotation = Quaternion.Euler(0,0,angleDeg);
            float distance = Vector2.Distance(_startPosition, position);
            var rendererSize = _renderer.size;
            rendererSize.x = Mathf.Max(0.5f, distance);
            _renderer.size = rendererSize;
            _renderer.transform.position = new Vector2(_startPosition.x  + (position.x - _startPosition.x) / 2,_startPosition.y + (position.y - _startPosition.y) / 2);
        }
    }
}