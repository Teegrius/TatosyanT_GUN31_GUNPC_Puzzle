using Core;
using SceneObjects;
using UnityEngine;

namespace RotateMechanics.GameField.IObjectManipulator
{
    public interface IObjectManipulator : IResetable, IInitializable<MainObject, TargetObject, MovePoint[], MovingZone[]>
    {
        void OnInputStart(Vector2 position);

        void OnInputHold(Vector2 position);

        void OnInputUp(Vector2 position);
    }
}