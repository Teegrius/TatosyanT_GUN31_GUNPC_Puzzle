namespace Core.Interfaces
{
    public interface ISettingsUser
    {
        ISettings CurrentSettings { get; }
        
        void SetSettings(ISettings settings);
    }
}