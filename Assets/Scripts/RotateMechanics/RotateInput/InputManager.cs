using Core.InputSource;
using Core.MessageSystem;
using Messages;
using RotateMechanics.RotateInput.InputType;
using UnityEngine;

namespace RotateMechanics.RotateInput
{
    public sealed class InputManager : MonoBehaviour, IMessageListener<SetInputActiveState>
    {
        private readonly IInputProcessor _inputProcessor = new RotateInputProcessor();
        private ITouchInputSource _inputSource;

        private void Awake()
        {
            _inputSource = Application.isMobilePlatform ? new TouchInput() : new MouseInput();
            _inputProcessor.Initialize(_inputSource);
            Messenger.Subscribe(this);
        }

        private void OnDestroy() => Messenger.Unsubscribe(this);

        private void Update() => _inputProcessor.ProcessInput();

        public void OnMessage(SetInputActiveState message) => enabled = message.IsActive;
    }
}