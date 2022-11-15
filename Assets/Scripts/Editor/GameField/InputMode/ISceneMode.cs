using UnityEditor;

namespace Editor.GameField.InputMode
{
    public interface ISceneMode
    {
        string Name { get; }

        void OnModeSelected() => Tools.current = Tool.View;
    }
}