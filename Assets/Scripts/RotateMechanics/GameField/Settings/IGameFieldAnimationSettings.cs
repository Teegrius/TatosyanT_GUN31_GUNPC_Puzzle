using Core.Interfaces;

namespace RotateMechanics.GameField.Settings
{
    public interface IGameFieldAnimationSettings : ISettings
    {
        float MoveSpeed { get; }
        float FieldRotationSpeed { get; }
        float XScaleFactor { get; }
        float YScaleFactor { get; }
    }
}