using System;
using Core.MessageSystem;
using Messages;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class GameUIController : MonoBehaviour
    {
        [SerializeField] private MenuController _menuController;
        [SerializeField] private Toggle _swipeToggle;

        private void Awake() => _swipeToggle.SetIsOnWithoutNotify(true);

        public void ShowMenu() => _menuController.Show();

        public void InputChangeMode(bool value)
        {
            if (_swipeToggle.isOn)
            {
                Messenger.Send(new SetSwipeInput());
            }
            else
            {
                Messenger.Send(new SetDragNDropInput());
            }
        }

        public void Restart() => Messenger.Send(new LevelRestarted());
    }
}
