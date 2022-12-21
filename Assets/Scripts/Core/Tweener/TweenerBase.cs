using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Tweener
{
    public abstract class TweenerBase : ITweener
    {
        private Action _onStart;
        private Action _onFinish;
        public bool IsPlaying { get; private set; }
        public float Duration { get; private set; }

        protected TweenerBase(float duration)
        {
            Duration = duration;
            SetDefault();
        }

        public async void Play()
        {
            try
            {
                if (Duration <= 0)
                {
                    PlayImmediately();
                    return;
                }

                IsPlaying = true;
                _onStart?.Invoke();
                await PlayAsync();
                _onFinish?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception while playing animation\n {e.Message}");
                Reset();
            }
            finally
            {
                IsPlaying = false;
            }
            
        }

        protected abstract Task PlayAsync();

        protected abstract void PlayImmediately();

        protected abstract void SetDefault();

        protected virtual void Reset() => IsPlaying = false;

        public ITweener WithDuration(float duration)
        {
            Duration = duration;
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