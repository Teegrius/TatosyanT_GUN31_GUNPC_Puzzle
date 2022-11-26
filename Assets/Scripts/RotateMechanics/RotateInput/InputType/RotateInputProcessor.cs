using Core.InputSource;
using Core.MessageSystem;
using Messages.Input;
using UnityEngine;

namespace RotateMechanics.RotateInput.InputType
{
    public sealed class RotateInputProcessor : IInputProcessor
    {
        private ITouchInputSource _inputSource;

        public void Initialize(ITouchInputSource inputSource) => _inputSource = inputSource;

        public void ProcessInput()
        {
            _inputSource.Check();
            if (_inputSource.IsDown)
            {
                OnInputStarted(_inputSource.InputPosition);
            }

            if (_inputSource.IsHold)
            {
                OnInputHold(_inputSource.InputPosition);
            }

            if (_inputSource.IsUp)
            {
                OnInputFinished(_inputSource.InputPosition);
            }
        }

        private void OnInputStarted(Vector2 position) => Messenger.Send(new InputStarted(position));

        private void OnInputHold(Vector2 position) => Messenger.Send(new InputHold(position));

        private void OnInputFinished(Vector2 position) => Messenger.Send(new InputFinished(position));
    }
}