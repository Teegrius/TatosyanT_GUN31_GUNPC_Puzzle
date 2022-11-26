using Core;
using Core.MessageSystem;
using Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public sealed class LevelButton : MonoBehaviour, IInitializable<int, int>
    {
        [SerializeField, LevelName] private string _levelName;
        private Button _button;
        private int _completedLevelNumber;
        private int _buttonIndex;

        public void Initialize(int completedLevelNumber, int index)
        {
            _completedLevelNumber = completedLevelNumber;
            _buttonIndex = index;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
            _button.GetComponentInChildren<TMP_Text>().text = _buttonIndex.ToString();
            CheckLevelCompleted();
        }

        private void OnButtonClicked()
        {
            if (_buttonIndex <= _completedLevelNumber || _buttonIndex - _completedLevelNumber == 1)
            {
                Messenger.Send(new LevelSelected(_levelName));
            }
        }

        private void CheckLevelCompleted()
        {
            if (_buttonIndex < _completedLevelNumber)
            {
                _button.GetComponent<Image>().color = Color.green;
            }
        }
    }
}