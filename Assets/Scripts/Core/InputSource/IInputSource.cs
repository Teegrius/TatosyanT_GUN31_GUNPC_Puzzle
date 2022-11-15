namespace Core.InputSource
{
    public interface IInputSource
    {
        bool IsDown { get; }
        bool IsHold { get; }
        bool IsUp { get; }

        void Check();
    }
}