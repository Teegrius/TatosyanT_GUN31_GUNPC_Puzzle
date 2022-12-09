using UnityEngine;

namespace SceneObjects
{
    public interface ISceneObject
    {
        Vector2 LocalPosition { get; }
        Transform Transform { get; }
    }
}