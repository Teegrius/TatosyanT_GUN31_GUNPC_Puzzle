using System.Threading;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

namespace Core.Tweener
{
    public sealed class ScaleTweener2D : TweenerBase
    {
        private readonly GameObject _gameObject;
        private readonly Vector3 _scale;
        private Vector3 _defaultScale;

        public ScaleTweener2D(GameObject gameObject, Vector2 scale, float duration = RotateConstants.One) : base(duration)
        {
            _gameObject = gameObject;
            _scale = scale;
            SetDefault();
        }

        protected override async Task PlayAsync(CancellationToken cancellationToken)
        {
            float t = RotateConstants.Zero;
            var oldScale = _gameObject.transform.localScale;
            while (t <= RotateConstants.One)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return;
                }

                t += Time.deltaTime / Duration;
                _gameObject.transform.localScale = Vector3.Lerp(oldScale, _scale, t);
                await Task.Yield();
            }
        }

        protected override void PlayImmediately() => _gameObject.transform.localScale = _scale;

        protected override void SetDefault() => _defaultScale = _gameObject.transform.localScale;

        public override void Reset() => _gameObject.transform.localScale = _defaultScale;
    }
}