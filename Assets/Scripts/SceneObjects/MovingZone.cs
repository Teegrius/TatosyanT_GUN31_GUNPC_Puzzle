using Core;
using DefaultNamespace;
using UnityEngine;

namespace SceneObjects
{
    public sealed class MovingZone : SceneObjectAbstract
    {
        private Vector2? _size;
        private SpriteRenderer _renderer;

        private Vector2 Size
        {
            get
            {
                _size ??= GetComponent<SpriteRenderer>().size;
                return _size.Value;
            }
        }

        private Renderer Renderer
        {
            get
            {
                _renderer ??= GetComponent<SpriteRenderer>();
                return _renderer;
            }
        }
        
        //TODO Design violation
        public override bool IsOnSamePosition(Vector2 position)
        {
            if (base.IsOnSamePosition(position))
            {
                return true;
            }
            
            if (transform.rotation == Quaternion.identity)
            {
                return position.x.IsBetweenRange(transform.position.x + Size.x * RotateConstants.Half, transform.position.x - Size.x * RotateConstants.Half) &&
                       position.y.IsBetweenRange(transform.position.y + Size.y * RotateConstants.Half, transform.position.y - Size.y * RotateConstants.Half);
            }

            return position.x.IsBetweenRange(transform.position.x + Size.y * RotateConstants.Half, transform.position.x - Size.y * RotateConstants.Half) &&
                   position.y.IsBetweenRange(transform.position.y + Size.x * RotateConstants.Half, transform.position.y - Size.x * RotateConstants.Half);

        }

        public bool HasIntersection(MovingZone zone) => zone.Renderer.bounds.Intersects(Renderer.bounds);
    }
}