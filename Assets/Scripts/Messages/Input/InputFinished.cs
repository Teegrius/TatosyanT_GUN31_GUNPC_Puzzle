using UnityEngine;

namespace Messages.Input
{
    public struct InputFinished
    {
        public readonly Vector2 Position;
        
        public InputFinished(Vector2 position) => Position = position;
    }
}