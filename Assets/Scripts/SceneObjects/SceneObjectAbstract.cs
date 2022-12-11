using UnityEngine;

namespace SceneObjects
{
    public abstract class SceneObjectAbstract : MonoBehaviour, ISceneObject
    {
        private const float Delta = 0.15f;
        private void Awake() => Transform = transform;

        public Vector2 LocalPosition => Transform.localPosition;
        public Transform Transform { get; private set; }

        public bool IsOnSamePosition(SceneObjectAbstract objectAbstract) =>
            IsOnSamePosition(objectAbstract.transform.position);
            
        public bool IsOnSamePosition(Vector2 position) =>
            Mathf.Abs(position.x - transform.position.x) <= Delta &&
            Mathf.Abs(position.y - transform.position.y) <= Delta;
    }
}