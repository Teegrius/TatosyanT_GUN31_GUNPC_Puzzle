using Core;
using UnityEngine;

namespace RotateMechanics.GameField.IObjectManipulator
{
    public interface IObjectManipulator : IResetable, IInitializable
    {
        void OnInputStart(Vector2 position);

        void OnInputHold(Vector2 position);

        void OnInputUp(Vector2 position);
    }
}