using DefaultNamespace;
using UnityEngine;
using UnityEngine.Windows;

namespace RotateMechanics.GameField
{
    [CreateAssetMenu(menuName = RotateConstants.RotateContextMenu + FileName, fileName = FileName)]
    public sealed class GameFieldAnimationSettings : ScriptableObject
    {
        private const string FileName = nameof(GameFieldAnimationSettings);
        [SerializeField, Range(1,4)] private float _moveSpeed;
        [SerializeField, Range(1,4)] private float _fieldRotationSpeed;
        [SerializeField, Range(0.1f,0.9f)] private float _xScaleFactor;
        [SerializeField, Range(0.1f,0.9f)] private float _yScaleFactor;
    }
}