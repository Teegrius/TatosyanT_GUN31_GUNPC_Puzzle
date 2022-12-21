using UnityEngine;

namespace Core.Tweener
{
    public static class TweenFactory
    {
        public static ITweener RotateAround(GameObject gameObject, float degrees, Vector3 axis) => new RotateAroundTweener(gameObject, degrees, axis);
        public static ITweener Scale2D(GameObject gameObject, Vector2 scale) => new ScaleTweener2D(gameObject, scale);

        public static ITweener Move2D(GameObject gameObject, Vector2 position) => new MoveTweener(gameObject, position);

        public static ITweener WithScale(this ITweener tweener, GameObject gameObject, Vector2 scale, float duration)
        {
            var scaleTween = Scale2D(gameObject, scale);
            scaleTween.WithDuration(duration);
            return new ParallelTweener(tweener, scaleTween);
        }
        
        public static ITweener WithMove(this ITweener tweener, GameObject gameObject, Vector2 position, float duration)
        {
            var scaleTween = Move2D(gameObject, position);
            scaleTween.WithDuration(duration);
            return new ParallelTweener(tweener, scaleTween);
        }
    }
}