using Core;
using Core.InputSource;
using Core.Interfaces;

namespace RotateMechanics.RotateInput.InputType
{
    public interface IInputProcessor : IInitializable<ITouchInputSource>
    {
        void ProcessInput();
    }
}