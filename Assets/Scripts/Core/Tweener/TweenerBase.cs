using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Tweener
{
    public abstract class TweenerBase : ITweener
    {
        private Action _onStart;
        private Action _onFinish;
        private CancellationTokenSource _cancellationTokenSource;
        public bool IsPlaying { get; private set; }
        public float Duration { get; private set; }

        protected TweenerBase(float duration)
        {
            Duration = duration;
            _cancellationTokenSource = new CancellationTokenSource();
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

                if (IsPlaying)
                {
                    Debug.LogError("Tweener is alreadpy playing");
                    return;
                }
                IsPlaying = true;
                _onStart?.Invoke();
                await PlayAsync().WithCancellation(_cancellationTokenSource.Token);
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

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            Reset();
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