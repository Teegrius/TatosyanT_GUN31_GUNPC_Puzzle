namespace Messages
{
    public struct LevelSelected
    {
        public readonly string LevelName;

        public LevelSelected(string levelName) => LevelName = levelName;
    }
}