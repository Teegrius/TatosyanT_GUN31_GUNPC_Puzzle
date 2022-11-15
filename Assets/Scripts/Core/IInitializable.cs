namespace Core
{
    public interface IInitializable
    {
        void Initialize();
    }
    
    public interface IInitializable<in TType>
    {
        void Initialize(TType type);
    }

    public interface IInitializable<in TType1, in TType2>
    {
        void Initialize(TType1 type1, TType2 type2);
    }
}