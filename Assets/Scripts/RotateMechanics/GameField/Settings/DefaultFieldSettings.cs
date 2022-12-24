using DefaultNamespace;

namespace RotateMechanics.GameField.Settings
{
    public sealed class DefaultFieldSettings : IGameFieldAnimationSettings
    {
        public float MoveSpeed => RotateConstants.One;
        public float FieldRotationSpeed => RotateConstants.One;
        public float XScaleFactor => RotateConstants.Half;
        public float YScaleFactor => RotateConstants.Half;
    }
}