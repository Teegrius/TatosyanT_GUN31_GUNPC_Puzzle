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
        }

        private void OnDestroy() => Messenger.Unsubscribe(this);

        private void LoadLevel(string levelName) => SceneManager.LoadScene(levelName);
        
        public void OnMessage(LevelSelected message) => LoadLevel(message.LevelName);
    }
}