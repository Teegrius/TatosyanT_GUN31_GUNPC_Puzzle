using UnityEngine;

namespace Core.InputSource
{
    public interface IVectorInputSource : IInputSource
    {
        Vector2 InputPosition { get; }
    }
}