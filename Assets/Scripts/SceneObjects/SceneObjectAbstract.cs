using UnityEngine;

namespace SceneObjects
{
    public abstract class SceneObjectAbstract : MonoBehaviour, ISceneObject
    {
        private const float Delta = 0.15f;
        public Vector2 LocalPosition => Transform.localPosition;
        public Transform Transform { get; private set; }
        
        private void Awake() => Transform = transform;
        
        public bool IsOnSamePosition(SceneObjectAbstract objectAbstract) =>
            IsOnSamePosition(objectAbstract.transform.position);
            
        public virtual bool IsOnSamePosition(Vector2 position) =>
            Mathf.Abs(position.x - transform.position.x) <= Delta &&
            Mathf.Abs(position.y - transform.position.y) <= Delta;
    }
}