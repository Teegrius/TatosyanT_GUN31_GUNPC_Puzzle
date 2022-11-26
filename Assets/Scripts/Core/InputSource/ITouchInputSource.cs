using UnityEngine;

namespace Core.InputSource
{
    public interface ITouchInputSource : IInputSource
    {
        Vector2 InputPosition { get; }
    }
}