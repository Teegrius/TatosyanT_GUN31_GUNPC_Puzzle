using System;

namespace Core.Tweener
{
    public interface ITweener  //TODO Decorator or use UniTask / Tween engine
    {
        bool IsPlaying { get; }
        float Duration { get;}
        void Play();
        void Stop();
        ITweener WithDuration(float duration);

        ITweener OnStart(Action action);

        ITweener OnFinish(Action action);
    }
}