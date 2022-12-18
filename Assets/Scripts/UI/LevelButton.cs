using System;
using Core.MessageSystem;
using Messages;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public sealed class LevelButton : MonoBehaviour
    {
        [SerializeField, LevelName] private string _levelName;
        private Button _button;
        private int _completedLevelNumber;
        private int _buttonIndex;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Messenger.Send(new LevelSelected(_levelName));
        }
    }
}