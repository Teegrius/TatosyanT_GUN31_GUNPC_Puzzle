using System;
using Core;
using Core.Interfaces;
using SceneObjects;
using UnityEngine;

namespace RotateMechanics.GameField.ObjectManipulators
{
    public interface IObjectManipulator : IResetable, IInitializable<MainObject, TargetObject, MovePoint[], MovingZone[]>, ISettingsUser, IDisposable
    {
        void OnInputStart(Vector2 position);

        void OnInputHold(Vector2 position);

        void OnInputUp(Vector2 position);
    }
}