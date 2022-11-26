using System;
using Core.MessageSystem;
using DefaultNamespace;
using Messages;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public sealed class MainMenuController : MonoBehaviour, IMessageListener<LevelSelected>
    {
        [SerializeField] private Transform _levelButtonsPanel;

        private void Awake()
        {
            Messenger.Subscribe(this);
            CheckFinishedLevels();
        }

        private void OnDestroy() => Messenger.Unsubscribe(this);

        private void CheckFinishedLevels()
        {
            int lastLevel = 0;
            int childCount = _levelButtonsPanel.childCount;
            if (PlayerPrefs.HasKey(RotateConstants.LevelPlayerPrefsKey))
            {
                lastLevel = PlayerPrefs.GetInt(RotateConstants.LevelPlayerPrefsKey);
                if (lastLevel - 1 > childCount)
                {
                    throw new Exception(
                        $"Last finished level number {lastLevel}, but the amount of buttons is {childCount}");
                }
            }

            for (int index = 0; index < childCount; index++)
            {
                var levelButton = _levelButtonsPanel.GetChild(index).GetComponent<LevelButton>();
                if (levelButton == null)
                {
                    throw new Exception(
                        $"There should be a button with {nameof(LevelButton)} script attached to it on in {_levelButtonsPanel.name} hierarchy");
                }
                levelButton.Initialize(lastLevel, index + 1);
            }
        }
        private void LoadLevel(string levelName) => SceneManager.LoadScene(levelName);
        
        public void OnMessage(LevelSelected message) => LoadLevel(message.LevelName);
    }
}