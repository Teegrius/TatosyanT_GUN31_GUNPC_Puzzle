using Core;
using SceneObjects;
using UnityEditor;
using UnityEngine;
using GameFieldManager = RotateMechanics.GameField.GameFieldManager;

namespace Editor.GameField.InputMode
{
    public sealed class SetMainMode : IEditSceneMode
    {
        private readonly GameFieldManager _gameFieldManager;

        public SetMainMode(GameFieldManager gameFieldManager) => _gameFieldManager = gameFieldManager;

        public string Name => "Set Main";

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

            if (_gameFieldManager.TargetObject != null &&
                movePoint.transform.position == _gameFieldManager.TargetObject.transform.position)
            {
                Debug.LogError("Can't put main object into Level Target object");
                return;
            }

            if (_gameFieldManager.MainObject == null)
            {
                var mainObject = Resources.Load("MainObject");
                var mainObjectInstance =
                    PrefabUtility.InstantiatePrefab(mainObject, _gameFieldManager.transform) as GameObject;
                _gameFieldManager.MainObject = mainObjectInstance.GetComponent<MainObject>();
            }

            _gameFieldManager.MainObject.transform.position = movePoint.transform.position;
        }
    }
}