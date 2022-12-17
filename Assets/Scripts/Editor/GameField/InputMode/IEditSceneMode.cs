using UnityEditor;
using UnityEngine;

namespace Editor.GameField.InputMode
{
    public interface IEditSceneMode : ISceneMode //TODO Visitor could be better
    {
        void ProcessInput(Vector2 mousePosition);
        
        void OnModeExit() => Tools.current = Tool.View;
    }
}