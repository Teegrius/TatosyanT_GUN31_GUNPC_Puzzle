using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Tweener
{
    public sealed class ParallelTweener : TweenerBase
    {
        private readonly ITweener[] _tweeners;

        public ParallelTweener(params ITweener[] tweeners) : base(1)
        {
            _tweeners = tweeners;
        }
        
        protected override async Task PlayAsync()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < _tweeners.Length; i++)
            {
                tasks.Add(new Task(_tweeners[i].Play));
            }

            await Task.WhenAll(tasks);
        }

        protected override void PlayImmediately()
        {
            
        }

        protected override void SetDefault()
        {
            
        }
    }
}