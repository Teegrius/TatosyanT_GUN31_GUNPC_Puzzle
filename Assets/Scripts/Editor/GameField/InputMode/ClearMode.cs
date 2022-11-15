using System;
using UnityEditor;

namespace Editor.GameField.InputMode
{
    public sealed class ClearMode : ISceneMode
    {
        private readonly Action _clearField;

        public string Name => "Clear";
        
        public ClearMode(Action clearField) => _clearField = clearField;

        public void OnModeSelected()
        {
            if (EditorUtility.DisplayDialog("Clear",
                    "Are you sure you want to clear field from all objects, except points?", "Yes", "No"))
            {
                _clearField?.Invoke();
            }
        }
    }
}