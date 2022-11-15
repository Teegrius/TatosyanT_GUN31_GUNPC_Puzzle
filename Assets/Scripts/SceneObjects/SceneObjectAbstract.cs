using UnityEngine;

namespace SceneObjects
{
    public abstract class SceneObjectAbstract : MonoBehaviour, ISceneObject
    {
        private void Awake() => Transform = transform;

        public Vector2 Position => Transform.position;
        public Transform Transform { get; private set; }
    }
}