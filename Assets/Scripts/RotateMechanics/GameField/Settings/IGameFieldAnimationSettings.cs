namespace RotateMechanics.GameField
{
    public interface IGameFieldAnimationSettings
    {
        float MoveSpeed { get; }
        float FieldRotationSpeed { get; }
        float XScaleFactor { get; }
        float YScaleFactor { get; }
    }
}