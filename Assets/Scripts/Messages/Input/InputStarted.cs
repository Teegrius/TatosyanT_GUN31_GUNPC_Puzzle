using UnityEngine;

namespace Messages.Input
{
    public struct InputStarted
    {
        public readonly Vector2 Position;

        public InputStarted(Vector2 position) => Position = position;
    }
}