using Core.InputSource;
using Core.MessageSystem;
using Messages;
using RotateMechanics.RotateInput.InputType;
using UnityEngine;

namespace RotateMechanics.RotateInput
{
    public sealed class InputManager : MonoBehaviour, IMessageListener<SetInputActiveState>
    {
        private readonly IVectorInputProcessor _inputProcessor = new RotateInputProcessor();
        private IVectorInputSource _inputSource;
        private bool _inputActive;

        private void Awake()
        {
            _inputSource = Application.isMobilePlatform ? new TouchInput() : new MouseInput();
            _inputActive = true;
            _inputProcessor.Initialize(_inputSource);
            Messenger.Subscribe(this);
        }

        private void OnDestroy() => Messenger.Unsubscribe(this);

        private void Update()
        {
            if (_inputActive)
            {
                _inputProcessor.ProcessInput();
            }
        }

        public void OnMessage(SetInputActiveState message) => _inputActive = message.IsActive;
    }
}