using System;
using System.Collections.Generic;
using SceneObjects;
using UnityEngine;

namespace RotateMechanics.GameField
{
    public sealed partial class GameFieldManager
    {
#if UNITY_EDITOR

        public const float GridSizeMin = 1f;
        public const float GridSizeMax = 3f;
        public const short MinGridValue = 3;
        public const short MaxGridValue = 10;
        private const float ClickDelta = 0.5f;
        private readonly Color _normalColor = Color.green;
        private readonly Color _selectedColor = Color.yellow;

        public TargetObject TargetObject
        {
            get => _targetObject;
            set => _targetObject = value;
        }

        public MainObject MainObject
        {
            get => _mainObject;
            set => _mainObject = value;
        }

        #region Grid Drawer

        private void OnDrawGizmos()
        {
            var oldColor = Gizmos.color;
            Gizmos.color = _normalColor;
            var xPosition = GetXGridValue();
            var yPosition = GetYGridValue();
            GridGizmo(xPosition, yPosition);
            GridFrameGizmo(xPosition, yPosition);
            Gizmos.color = oldColor;
        }

        private void OnDrawGizmosSelected()
        {
            var oldColor = Gizmos.color;
            Gizmos.color = _selectedColor;
            GridFrameGizmo(GetXGridValue(), GetYGridValue());
            Gizmos.color = oldColor;
        }

        private void GridFrameGizmo(float x, float y)
        {
            Gizmos.DrawLine(new Vector3(-x, -y, 0), new Vector3(-x, y, 0));
            Gizmos.DrawLine(new Vector3(-x, y, 0), new Vector3(x , y, 0));
            Gizmos.DrawLine(new Vector3(x, y, 0), new Vector3(x, -y, 0));
            Gizmos.DrawLine(new Vector3(x, -y, 0), new Vector3(-x, -y, 0));
        }

        private void GridGizmo(float x, float y)
        {
            for (var col = -x; col <= x; col+= _gridSize)
            {
                Gizmos.DrawLine(new Vector3(col, -y, 0), new Vector3(col, y, 0));
            }

            for (var row = -y; row <= y; row++)
            {
                Gizmos.DrawLine(new Vector3(-x, row, 0), new Vector3(x, row, 0));
            }
        }

        #endregion

        #region Grid Calculation

        public bool TryGetMovePoint(Vector2 point, out MovePoint movePoint)
        {
            for (var i = 0; i < _movePoints.Length; i++)
            {
                if (Mathf.Abs(_movePoints[i].transform.position.x - point.x) <= ClickDelta &&
                    Mathf.Abs(_movePoints[i].transform.position.y - point.y) <= ClickDelta)
                {
                    movePoint = _movePoints[i];
                    return true;
                }
            }

            movePoint = default;
            return false;
        }

        public bool TryGetMovingZone(Vector2 point, out MovingZone movingZone)
        {
            for (var i = 0; i < _movingZones.Length; i++)
            {
                if (Mathf.Abs(_movingZones[i].transform.position.x - point.x) <= ClickDelta &&
                    Mathf.Abs(_movingZones[i].transform.position.y - point.y) <= ClickDelta)
                {
                    movingZone = _movingZones[i];
                    return true;
                }
            }

            movingZone = default;
            return false;
        }

        public float GetXGridValue() => _gridSize * (_totalColumns / 2) + _gridSize / 2 * (_totalColumns % 2 - 1);

        public float GetYGridValue() => _gridSize * (_totalRows / 2) + (_gridSize / 2) * (_totalRows % 2 - 1);

        public bool TryAddStar(MovePoint point,out string errorMessage)
        {
            errorMessage = string.Empty;
            if (_mainObject.Position == point.Position)
            {
                errorMessage = "Can't add star above main object";
                return false;
            }
            if (_targetObject.Position == point.Position)
            {
                errorMessage = "Can't add star above target object";
                return false;
            }

            if (_stars is { Count: >= MaxStarsCount })
            {
                errorMessage = "Can't add more than three stars!";
                return false;
            }

            _stars ??=new List<StarObject>();
            var star = Instantiate(Resources.Load("Star")) as GameObject;
            star.transform.position = point.Position;
            star.transform.parent = transform;
            _stars.Add(star.GetComponent<StarObject>());
            return true;
        }

        #endregion
#endif
    }
}