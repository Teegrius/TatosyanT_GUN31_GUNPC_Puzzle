using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Editor.GameField.InputMode;
using SceneObjects;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using GameFieldManager = RotateMechanics.GameField.GameFieldManager;

namespace Editor.GameField
{
    [CustomEditor(typeof(GameFieldManager))]
    public sealed class GameFieldEditor : UnityEditor.Editor
    {
        private GameFieldManager _gameFieldManager;
        private float _gridSize;
        private int _columns;
        private int _rows;
        private Transform _movePoints;
        private Transform _movingZones;

        private float _gridSizeTemp;
        private int _columnsTemp;
        private int _rowsTemp;

        private IEditSceneMode _editSceneMode;
        private bool _foldoutOpen;
        private int _sceneModeIndex;
        private List<ISceneMode> _sceneModes;
        private string[] _modesNames;

        private SerializedProperty _columnsProperty;
        private SerializedProperty _rowsProperty;
        private SerializedProperty _gridSizeProperty;
        private SerializedProperty _movePointsProperty;

        private void OnEnable()
        {
            SetInitialValues();
            if (_columns * _rows != _movePointsProperty.arraySize)
            {
                CreateMovingPoints();
            }
            
            void SetInitialValues()
            {
                _gameFieldManager = (GameFieldManager)target;
                _movePoints = _gameFieldManager.transform.Find(RotateConstants.MovePoints);
                _movingZones = _gameFieldManager.transform.Find(RotateConstants.MovingZones);
                _columnsProperty = serializedObject.FindProperty("_totalColumns");
                _rowsProperty = serializedObject.FindProperty("_totalRows");
                _gridSizeProperty = serializedObject.FindProperty("_gridSize");
                _movePointsProperty = serializedObject.FindProperty("_movePoints");
                _columns = _columnsTemp = _columnsProperty.intValue;
                _rows = _rowsTemp = _rowsProperty.intValue;
                _gridSize = _gridSizeTemp = _gridSizeProperty.floatValue;
                _sceneModes ??= new List<ISceneMode>
                {
                    new ViewMode(),
                    new DrawMovingZonesMode(_gameFieldManager, CreateMovingZone),
                    new EraseMovingZonesMode(_gameFieldManager, EraseMovingZone),
                    new SetMainMode(_gameFieldManager),
                    new SetTargetMode(_gameFieldManager),
                    new DrawStarsMode(_gameFieldManager),
                    new EraseStarMode(_gameFieldManager),
                    new ClearMode(ClearLevelObjects)
                };
                _modesNames ??= _sceneModes.Select(t => t.Name).ToArray();
            }
        }

        private void CreateMovingPoints()
        {
            ClearProperty("_movePoints");
            ClearLevelObjects();
            InitializeMovePointsArray(_columns * _rows);
            var obj = Resources.Load("MovePoint");
            var index = 0;
            var x = _gameFieldManager.GetXGridValue();
            var y = _gameFieldManager.GetYGridValue();
            for (var col = -x; col <= x; col+=_gridSize)
            {
                for (var row = -y; row <= y; row+=_gridSize)
                {
                    var instance = Instantiate(obj) as GameObject;
                    instance.transform.position = new Vector3(col, row, 0);
                    instance.transform.parent = _movePoints;
                    _movePointsProperty.GetArrayElementAtIndex(index).objectReferenceValue =
                        instance.GetComponent<MovePoint>();
                    serializedObject.ApplyModifiedPropertiesWithoutUndo();
                    index++;
                }
            }
            
            void InitializeMovePointsArray(int count)
            {
                for (var i = 0; i < count; i++)
                {
                    _movePointsProperty.InsertArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
        }

        private void CreateMovingZone(MovingZone movingZone)
        {
            movingZone.transform.parent = _movingZones;
            var property = serializedObject.FindProperty("_movingZones");
            property.InsertArrayElementAtIndex(property.arraySize);
            property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = movingZone;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private void EraseMovingZone(MovingZone movingZone)
        {
            var property = serializedObject.FindProperty("_movingZones");
            for (var i = 0; i < property.arraySize; i++)
            {
                if (property.GetArrayElementAtIndex(i).objectReferenceValue is MovingZone zone &&
                    movingZone.Equals(zone))
                {
                    property.DeleteArrayElementAtIndex(i);
                    DestroyImmediate(movingZone.gameObject);
                    serializedObject.ApplyModifiedPropertiesWithoutUndo();
                    break;
                }
            }
        }

        private void ClearLevelObjects()
        {
            ClearProperty("_mainObject");
            ClearProperty("_targetObject");
            ClearProperty("_movingZones");
            ClearProperty("_stars");
        }

        private void ClearProperty(string propertyName)
        {
            var serializedProperty = serializedObject.FindProperty(propertyName);
            if (serializedProperty.isArray)
            {
                for (var i = serializedProperty.arraySize - 1; i >= 0; i--)
                {
                    var value = serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                    serializedProperty.DeleteArrayElementAtIndex(i);
                    DestroyImmediate(value.GameObject());
                }
            }
            else
            {
                DestroyImmediate(serializedProperty.objectReferenceValue.GameObject());
                serializedProperty.objectReferenceValue = null;
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private void OnSceneGUI()
        {
            if (Application.isPlaying)
            {
                return;
            }
            
            DrawModeTools();
            EventHandler();

            void DrawModeTools()
            {
                Handles.BeginGUI();
                GUILayout.BeginArea(new Rect(100f, 10f, 800, 40f));
                var index = GUILayout.Toolbar(
                    _sceneModeIndex,
                    _sceneModes.Select(t => t.Name).ToArray(),
                    GUILayout.ExpandHeight(true));
                if (index != _sceneModeIndex)
                {
                    _sceneModeIndex = index;
                    var sceneMode = _sceneModes[_sceneModeIndex];
                    sceneMode.OnModeSelected();
                    if (sceneMode is IEditSceneMode editSceneMode)
                    {
                        _editSceneMode = editSceneMode;
                    }
                    else
                    {
                        _editSceneMode = null;
                    }
                }

                GUILayout.EndArea();
                Handles.EndGUI();
            }
        }

        public override void OnInspectorGUI()
        {
            DrawLevelParameters();
            DrawCoreParameters();
            CheckGameObjectPosition();
        }

        private void DrawLevelParameters()
        {
            EditorGUILayout.BeginVertical();
            
            _columnsTemp = EditorGUILayout.IntSlider("Columns", _columnsTemp, GameFieldManager.MinGridValue,
                GameFieldManager.MaxGridValue, GUILayout.ExpandWidth(true));
            _rowsTemp = EditorGUILayout.IntSlider("Rows", _rowsTemp, GameFieldManager.MinGridValue,
                GameFieldManager.MaxGridValue, GUILayout.ExpandWidth(true));
            _gridSizeTemp = EditorGUILayout.Slider("Grid Size", _gridSizeTemp, GameFieldManager.GridSizeMin, GameFieldManager.GridSizeMax, GUILayout.ExpandWidth(true));
            if (_columnsTemp != _columns || _rowsTemp != _rows || _gridSizeTemp != _gridSize)
            {
                if (Application.isPlaying)
                {
                    Debug.LogError("Can't edit Game Field in play mode");
                    ResetValues();
                    return;
                    
                }
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Save", GUILayout.ExpandWidth(true)))
                {
                    SetNewLevelValues();
                    CreateMovingPoints();
                }
            
                if (GUILayout.Button("Reset"))
                {
                    ResetValues();
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();

            void SetNewLevelValues()
            {
                _columns = _columnsTemp;
                _rows = _rowsTemp;
                _gridSize = _gridSizeTemp;

                _columnsProperty.intValue = _columns;
                _rowsProperty.intValue = _rows;
                _gridSizeProperty.floatValue = _gridSize;
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            void ResetValues()
            {
                _columnsTemp = _columns;
                _rowsTemp = _rows;
                _gridSizeTemp = _gridSize;
            }
        }

        private void DrawCoreParameters()
        {
            if (Application.isPlaying)
            {
                return;
            }
            
            _foldoutOpen = EditorGUILayout.Foldout(_foldoutOpen, "Core objects");
            if (_foldoutOpen)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_mainObject"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_targetObject"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_movePoints"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_movingZones"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_stars"));
            }
        }

        private void CheckGameObjectPosition()
        {
            if (_gameFieldManager.transform.localPosition != Vector3.zero)
            {
                _gameFieldManager.transform.localPosition = Vector3.zero;
            }
        }

        private void EventHandler()
        {
            HandleUtility.AddDefaultControl(
                GUIUtility.GetControlID(FocusType.Passive));
            var mousePosition = Event.current.mousePosition;
            var worldPos = HandleUtility.GUIPointToWorldRay(mousePosition);
            _editSceneMode?.ProcessInput(worldPos.origin);
        }
    }
}