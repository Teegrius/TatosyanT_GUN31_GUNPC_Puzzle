using System;
using System.IO;
using System.Linq;
using UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(LevelName))]
    public sealed class LevelPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType != typeof(string))
            {
                Debug.LogError($"Attribute {nameof(LevelName)} should only be applied to {typeof(string)} field type");
                return;
            }

            var scenes = GetScenes();
            var index = Mathf.Max(0, Array.IndexOf(scenes, property.stringValue));
            index = EditorGUI.Popup(position, "Level", index, scenes);
            property.stringValue = scenes[index];
        }

        private string[] GetScenes() => EditorBuildSettings.scenes.Select(GetLevelName()).ToArray();

        private static Func<EditorBuildSettingsScene, string> GetLevelName() => settingsScene => Path.GetFileNameWithoutExtension(settingsScene.path.Substring(settingsScene.path.LastIndexOf('/') + 1));
    }
}