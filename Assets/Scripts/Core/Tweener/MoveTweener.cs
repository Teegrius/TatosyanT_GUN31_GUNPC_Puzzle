using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

namespace Core.Tweener
{
    public sealed class MoveTweener : TweenerBase
    {
        private readonly GameObject _gameObject;
        private readonly Vector3 _position;
        private Vector3 _defaultPosition;

        public MoveTweener(GameObject gameObject, Vector3 position) : base(0)
        {
            _gameObject = gameObject;
            _position = position;
        }
        
        protected override async Task PlayAsync()
        {
            float t = RotateConstants.Zero;
            while (t <= 1.1f)
            {
                var oldPosition = _gameObject.transform.position;
                _gameObject.transform.position = Vector3.Lerp(oldPosition, _position, t);
                t += Time.deltaTime * Duration;
                await Task.Yield();
            }
        }

        protected override void PlayImmediately() => _gameObject.transform.position = _position;

        protected override void SetDefault() => _defaultPosition = _gameObject.transform.position;

        protected override void Reset()
        {
            base.Reset();
            _gameObject.transform.position = _defaultPosition;
        }
    }
}