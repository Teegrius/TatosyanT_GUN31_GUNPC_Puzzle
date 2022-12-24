using Core.MessageSystem;
using Messages;
using Messages.Input;
using Unity.VisualScripting;

namespace RotateMechanics.GameField
{
    public interface IGameFieldManager : IInitializable,
        IMessageListener<LevelRestarted>, 
        IMessageListener<InputStarted>,
        IMessageListener<InputHold>,
        IMessageListener<InputFinished>
    {
        void Subscribe();
        void Unsubscribe();
    }
}