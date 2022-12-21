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

        public ScaleTweener2D(GameObject gameObject, Vector2 scale) : base(0)
        {
            _gameObject = gameObject;
            _scale = scale;
        }

        protected override async Task PlayAsync()
        {
            float t = RotateConstants.Zero;
            while (t <= 1.1f)
            {
                var oldScale = _gameObject.transform.localScale;
                _gameObject.transform.localScale = Vector3.Lerp(oldScale, _scale, t);
                t += Time.deltaTime * Duration;
                await Task.Yield();
            }
        }

        protected override void PlayImmediately() => _gameObject.transform.localScale = _scale;

        protected override void SetDefault() => _defaultScale = _gameObject.transform.localScale;

        protected override void Reset() => _gameObject.transform.localScale = _defaultScale;
    }
}