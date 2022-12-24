namespace Core.Interfaces
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
    
    public interface IInitializable<in TType1, in TType2, in TType3, in TType4>
    {
        void Initialize(TType1 type1, TType2 type2, TType3 type3, TType4 type4);
    }
}