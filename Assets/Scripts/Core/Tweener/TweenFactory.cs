using Core.Tweener.Decorator;
using UnityEngine;

namespace Core.Tweener
{
    public static class TweenFactory
    {
        public static ITweener RotateAround(GameObject gameObject, float degrees, Vector3 axis, float duration = 1) => new RotateAroundTweener(gameObject, degrees, axis, duration);
        
        public static ITweener Scale2D(GameObject gameObject, Vector2 scale, float duration = 1) => new ScaleTweener2D(gameObject, scale, duration);
        
        public static ITweener Move2D(GameObject gameObject, Vector2 position, float duration = 1) => new MoveTweener(gameObject, position, duration);
        
        public static ITweener WithScale(this ITweener tweener, GameObject gameObject, Vector2 scale, float duration)
        {
            var scaleTween = Scale2D(gameObject, scale, duration);
            scaleTween.WithDuration(duration);
            return new ParallelTweener(tweener, scaleTween);
        }
        
        public static ITweener WithMove(this ITweener tweener, GameObject gameObject, Vector2 position, float duration)
        {
            var moveTween = Move2D(gameObject, position);
            moveTween.WithDuration(duration);
            return new ParallelTweener(tweener, moveTween);
        }
        
        public static ITweener WithRotation(this ITweener tweener, GameObject gameObject, float degrees, Vector3 axis, float duration)
        {
            var moveTween = RotateAround(gameObject, degrees, axis);
            moveTween.WithDuration(duration);
            return new ParallelTweener(tweener, moveTween);
        }

        public static ITweener ThenScale(this ITweener tweener, GameObject gameObject, Vector2 scale, float duration)
        {
            var scaleTween = Scale2D(gameObject, scale, duration);
            scaleTween.WithDuration(duration);
            return new SequenceTweener(tweener, scaleTween);
        }

        public static ITweener CreateSequence(ITweener tweener1, ITweener tweener2) => new SequenceTweener(tweener1, tweener2);
        
        public static ITweener CreateParallel(ITweener tweener1, ITweener tweener2) => new ParallelTweener(tweener1, tweener2);
        
        public static ITweener WithSequence(this ITweener tweener, ITweener tweener1, ITweener tweener2) => new ParallelTweener(tweener, CreateSequence(tweener1, tweener2));
        
        public static ITweener WithParallel(this ITweener tweener, ITweener tweener1, ITweener tweener2) => new ParallelTweener(tweener, CreateParallel(tweener1, tweener2));
    }
}