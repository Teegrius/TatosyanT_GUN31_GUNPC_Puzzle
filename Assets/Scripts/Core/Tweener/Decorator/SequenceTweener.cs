using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Tweener.Decorator
{
    public sealed class SequenceTweener : ITweener
    {
        private readonly ITweener _firstTweener;
        private readonly ITweener _secondTweener;
        private Action _onStart;
        private Action _onFinish;
        private CancellationTokenSource _source;

        public SequenceTweener(ITweener firstTweener, ITweener secondTweener)
        {
            _firstTweener = firstTweener;
            _secondTweener = secondTweener;
        }

        public bool IsPlaying { get; private set; }
        public float Duration => _firstTweener.Duration + _secondTweener.Duration;
        public async void Play()
        {
            try
            {
                _source = new CancellationTokenSource();
                IsPlaying = true;
                _onStart?.Invoke();
                await PlaySequence(_source.Token);
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                _onFinish?.Invoke();
                IsPlaying = false;
            }
        }

        private async Task PlaySequence(CancellationToken cancellationToken)
        {
            _firstTweener.Play();
            await Task.Delay(TimeSpan.FromSeconds(_firstTweener.Duration), cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return;
            }
            _secondTweener.Play();
            await Task.Delay(TimeSpan.FromSeconds(_secondTweener.Duration),cancellationToken);
            _onFinish?.Invoke();
            IsPlaying = false;
        }

        public void Stop()
        {
            if (!IsPlaying)
            {
                return;
            }
            _source?.Cancel();
            _firstTweener.Stop();
            _secondTweener.Stop();
            Reset();
        }

        public void Reset()
        {
            _firstTweener.Reset();
            _secondTweener.Reset();
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