using Unity.VisualScripting;
using UnityEngine;

namespace SceneObjects
{
    public interface ISceneObject : IInitializable
    {
        Vector2 Position { get; }
        Transform Transform { get; }
    }
}