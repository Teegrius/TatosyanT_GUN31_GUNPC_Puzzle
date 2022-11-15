using UnityEngine;

namespace Messages.Input
{
    public struct InputHold
    {
        public readonly Vector2 Position;

        public InputHold(Vector2 position) => Position = position;
    }
}