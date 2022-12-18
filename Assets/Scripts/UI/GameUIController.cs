using Core.MessageSystem;
using Messages;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class GameUIController : MonoBehaviour, IMessageListener<LevelCompleted>
    {
        [SerializeField] private MenuController _menuController;
        [SerializeField] private Toggle _swipeToggle;
        [SerializeField] private VictoryMenuController _victoryMenuController;

        private void Awake()
        {
            _swipeToggle.SetIsOnWithoutNotify(true);
            _victoryMenuController.gameObject.SetActive(false);
            Messenger.Subscribe(this);
        }

        private void OnDestroy() => Messenger.Unsubscribe(this);

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
        
        public void OnMessage(LevelCompleted message) => _victoryMenuController.gameObject.SetActive(true);
    }
}
