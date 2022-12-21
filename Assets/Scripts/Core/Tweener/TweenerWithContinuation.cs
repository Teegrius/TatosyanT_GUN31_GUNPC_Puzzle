using System.Threading.Tasks;

namespace Core.Tweener
{
    public sealed class TweenerWithContinuation : TweenerBase
    {
        private readonly ITweener _firstTweener;
        private readonly ITweener _secondTweener;

        public TweenerWithContinuation(ITweener firstTweener, ITweener secondTweener) : base(1)
        {
            _firstTweener = firstTweener;
            _secondTweener = secondTweener;
        }

        protected override async Task PlayAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override void PlayImmediately()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetDefault()
        {
            throw new System.NotImplementedException();
        }
    }
}