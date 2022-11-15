using System;
using System.Linq;
using DefaultNamespace;
using RotateMechanics;
using RotateMechanics.RotateInput;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using GameFieldManager = RotateMechanics.GameField.GameFieldManager;
using Object = UnityEngine.Object;

namespace Editor
{
    public class RotateMenu
    {
        [MenuItem("Rotate/CreateLevel")]
        public static void CreateGameFieldManager()
        {
            var scene = SceneManager.GetActiveScene();
            if (scene.GetRootGameObjects().Any(t => t.GetComponent<GameController>() != null || t.GetComponent<GameFieldManager>() != null || t.GetComponent<InputManager>()))
            {
                throw new Exception("Scene already contains GameFieldManager or Input Manager");
            }
        
            var gameController = new GameObject("Game").AddComponent<GameController>();
            var fieldManager = new GameObject("GameFieldManager").AddComponent<GameFieldManager>();
            var inputManager = new GameObject("InputManager").AddComponent<InputManager>();
            var fieldManagerTransform = fieldManager.transform;
            var movePoints = new GameObject(RotateConstants.MovePoints)
            {
                transform =
                {
                    parent = fieldManagerTransform,
                    localPosition = Vector3.zero
                }
            };
        
            var movingZones = new GameObject(RotateConstants.MovingZones)
            {
                transform =
                {
                    parent = fieldManagerTransform,
                    localPosition = Vector3.zero
                }
            };
            var menuObject = Resources.Load("UI");
            PrefabUtility.InstantiatePrefab(menuObject, scene);

            var eventSystem = new GameObject("Event System").AddComponent<EventSystem>();
            eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            
            Undo.RegisterCreatedObjectUndo(gameController.gameObject, "Undo Game Controller");
            Undo.RegisterCreatedObjectUndo(fieldManager.gameObject, "Undo field manager");
            Undo.RegisterCreatedObjectUndo(inputManager.gameObject, "Undo input");
            Undo.RegisterCreatedObjectUndo(movePoints, "Undo");
            Undo.RegisterCreatedObjectUndo(movingZones, "Undo");
            Undo.RegisterCreatedObjectUndo(menuObject, "Undo Menu");
            Undo.RegisterCreatedObjectUndo(eventSystem.gameObject, "Undo Event System");
        }
    
        [MenuItem("Rotate/Clear")]
        public static void ClearLevel()
        {
            var scene = SceneManager.GetActiveScene();
            Debug.LogError($"Scene name {scene.name}");
            var gameObjects = scene.GetRootGameObjects().ToList();
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                var go = gameObjects[i];
                if (go.GetComponent<Camera>() != null)
                {
                    continue;
                }
                Debug.LogError($"Destroy {go.name}");
                Object.DestroyImmediate(go);
                gameObjects.RemoveAt(i);
            }
        }
    }
}
