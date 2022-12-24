using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

namespace Core.Tweener
{
    public sealed class RotateAroundTweener : TweenerBase
    {
        private readonly GameObject _gameObject;
        private readonly float _degrees;
        private readonly Vector3 _axis;
        private Quaternion _defaultRotation;

        public RotateAroundTweener(GameObject gameObject, float degrees, Vector3 axis, float duration = 1) : base(duration)
        {
            _gameObject = gameObject;
            _degrees = degrees;
            _axis = axis;
            SetDefault();
        }


        protected override async Task PlayAsync()
        {
            var oldRotation = _gameObject.transform.localRotation;
            var newRotation = oldRotation * Quaternion.Euler(_axis * _degrees);
            float t = RotateConstants.Zero;
            while (t <= 1.1f)
            {
                _gameObject.transform.localRotation = Quaternion.Lerp(oldRotation, newRotation, t);
                t += Time.deltaTime / Duration;
                await Task.Yield();
            }
        }

        protected override void PlayImmediately() => _gameObject.transform.localRotation *= Quaternion.Euler(_axis * _degrees);

        protected override void SetDefault() => _defaultRotation = _gameObject.transform.localRotation;

        protected override void Reset()
        {
            base.Reset();
            _gameObject.transform.localRotation = _defaultRotation;
        }
    }
}