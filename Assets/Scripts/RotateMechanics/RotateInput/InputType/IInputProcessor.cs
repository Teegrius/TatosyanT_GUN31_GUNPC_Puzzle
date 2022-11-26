using Core;
using Core.InputSource;

namespace RotateMechanics.RotateInput.InputType
{
    public interface IInputProcessor : IInitializable<ITouchInputSource>
    {
        void ProcessInput();
    }
}