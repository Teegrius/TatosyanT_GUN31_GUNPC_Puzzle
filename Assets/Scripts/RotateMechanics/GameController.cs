using Core.MessageSystem;
using Messages;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RotateMechanics
{
    public sealed class GameController : MonoBehaviour, IMessageListener<LevelCompleted>
    {
        private AudioSource _audioSource;
        
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

        public void OnMessage(LevelCompleted message) => SceneManager.LoadScene("Main");
    }
}