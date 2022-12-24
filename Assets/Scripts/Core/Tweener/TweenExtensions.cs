using System.Threading;
using System.Threading.Tasks;

namespace Core.Tweener
{
    public static class TweenExtensions
    {
        public struct Void { }
        
        public static async Task WithCancellation(this Task originalTask, CancellationToken ct)
        {
            var cancelTask = new TaskCompletionSource<Void>();
            await using (ct.Register(t => ((TaskCompletionSource<Void>)t).TrySetResult(new Void()), cancelTask))
            {
                var any = await Task.WhenAny(originalTask, cancelTask.Task);
                if (any == cancelTask.Task)
                {
                    ct.ThrowIfCancellationRequested();
                    return;
                }
            }
            await originalTask;
        }
    }
}