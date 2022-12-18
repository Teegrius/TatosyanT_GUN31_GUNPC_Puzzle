using Core.MessageSystem;
using DefaultNamespace;
using Messages;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RotateMechanics
{
    public sealed class GameController : MonoBehaviour, IMessageListener<OpenNextLevel>
    {
        private AudioSource _audioSource;
        [SerializeField, LevelName] private string _levelName;
        
        private void Awake()
        {
            InitializeAudio();

            void InitializeAudio()
            {
                _audioSource = new GameObject("Audio").AddComponent<AudioSource>();
            }
            
            Messenger.Subscribe(this);
        }

        private void OnDestroy() => Messenger.Unsubscribe(this);

        public void OnMessage(OpenNextLevel message) => SceneManager.LoadScene(_levelName);
    }
}