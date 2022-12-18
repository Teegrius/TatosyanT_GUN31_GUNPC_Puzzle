using UnityEngine;

namespace SceneObjects
{
    public sealed class MovingZone : SceneObjectAbstract
    {
        private Vector2? _size;

        public Vector2 Size
        {
            get
            {
                _size ??= GetComponent<SpriteRenderer>().size;
                return _size.Value;
            }
        }
        
        //TODO Design violation
        public override bool IsOnSamePosition(Vector2 position)
        {
            if (transform.rotation == Quaternion.identity)
            {
                return transform.position.x - Size.x < position.x &&
                       transform.position.x + Size.x > position.x
                       && transform.position.y - Size.y < position.y &&
                       transform.position.y + Size.y > position.y;
            }

            return transform.position.x - Size.y < position.x &&
                   transform.position.x + Size.y > position.x
                   && transform.position.y - Size.x < position.y &&
                   transform.position.y + Size.x > position.y;

        }
    }
}