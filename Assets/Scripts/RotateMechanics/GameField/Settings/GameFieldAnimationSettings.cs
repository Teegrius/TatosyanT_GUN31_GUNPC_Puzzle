using DefaultNamespace;
using UnityEngine;

namespace RotateMechanics.GameField.Settings
{
    [CreateAssetMenu(menuName = RotateConstants.RotateContextMenu + FileName, fileName = FileName)]
    public sealed class GameFieldAnimationSettings : ScriptableObject, IGameFieldAnimationSettings
    {
        private const string FileName = nameof(GameFieldAnimationSettings);
        [SerializeField, Range(0.3f,4)] private float _moveSpeed = 1;
        [SerializeField, Range(0.5f,4)] private float _fieldRotationSpeed = 1;
        [SerializeField, Range(0.1f,1.3f)] private float _xScaleFactor = 1;
        [SerializeField, Range(0.1f,1.3f)] private float _yScaleFactor = 1;

        public float MoveSpeed => _moveSpeed;
        public float FieldRotationSpeed => _fieldRotationSpeed;
        public float XScaleFactor => _xScaleFactor;
        public float YScaleFactor => _yScaleFactor;
    }
}