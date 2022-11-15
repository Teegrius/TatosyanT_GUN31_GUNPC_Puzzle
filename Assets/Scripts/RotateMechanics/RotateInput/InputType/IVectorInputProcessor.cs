using Core;
using Core.InputSource;

namespace RotateMechanics.RotateInput.InputType
{
    public interface IVectorInputProcessor : IInitializable<IVectorInputSource>
    {
        void ProcessInput();
    }
}