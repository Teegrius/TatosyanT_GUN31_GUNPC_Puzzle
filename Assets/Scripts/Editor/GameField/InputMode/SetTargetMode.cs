using Core;
using SceneObjects;
using UnityEditor;
using UnityEngine;
using GameFieldManager = RotateMechanics.GameField.GameFieldManager;

namespace Editor.GameField.InputMode
{
    public sealed class SetTargetMode : IEditSceneMode
    {
        private readonly GameFieldManager _gameFieldManager;

        public SetTargetMode(GameFieldManager gameFieldManager)
        {
            _gameFieldManager = gameFieldManager;
        }

        public string Name => "Set Level Target";

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

            if (_gameFieldManager.MainObject != null &&
                movePoint.transform.position == _gameFieldManager.MainObject.transform.position || _gameFieldManager.IsPointInsideStar(movePoint))
            {
                Debug.LogError("Can't put Level Target object into other Object");
                return;
            }

            if (_gameFieldManager.TargetObject == null)
            {
                var targetObject = Resources.Load("TargetObject");
                var targetObjectInstance =
                    PrefabUtility.InstantiatePrefab(targetObject, _gameFieldManager.transform) as GameObject;
                _gameFieldManager.TargetObject = targetObjectInstance.GetComponent<TargetObject>();
            }

            _gameFieldManager.TargetObject.transform.position = movePoint.transform.position;
        }
    }
}