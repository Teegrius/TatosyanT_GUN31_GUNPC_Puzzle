using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Tweener
{
    public sealed class TweenerWithContinuation : ITweener
    {
        private readonly ITweener _firstTweener;
        private readonly ITweener _secondTweener;
        private Action _onStart;
        private Action _onFinish;

        public TweenerWithContinuation(ITweener firstTweener, ITweener secondTweener)
        {
            _firstTweener = firstTweener;
            _secondTweener = secondTweener;
        }

        public bool IsPlaying { get; private set; }
        public float Duration => _firstTweener.Duration + _secondTweener.Duration;
        public async void Play()
        {
            IsPlaying = true;
            _onStart?.Invoke();
            _firstTweener.Play();
            await Task.Delay(TimeSpan.FromSeconds(_firstTweener.Duration));
            _secondTweener.Play();
            await Task.Delay(TimeSpan.FromSeconds(_secondTweener.Duration));
            _onFinish?.Invoke();
            IsPlaying = false;
        }

        public void Stop()
        {
            _firstTweener.Stop();
            _secondTweener.Stop();
        }

        public ITweener WithDuration(float duration)
        {
            return this;
        }

        public ITweener OnStart(Action action)
        {
            _onStart = action;
            return this;
        }

        public ITweener OnFinish(Action action)
        {
            _onFinish = action;
            return this;
        }
    }
}