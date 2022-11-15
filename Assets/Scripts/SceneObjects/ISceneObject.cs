using UnityEngine;

namespace SceneObjects
{
    public interface ISceneObject
    {
        Vector2 Position { get; }
        Transform Transform { get; }
    }
}