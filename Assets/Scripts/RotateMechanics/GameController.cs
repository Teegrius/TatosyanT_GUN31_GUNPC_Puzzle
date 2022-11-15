using Core.MessageSystem;
using Messages;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace RotateMechanics
{
    public sealed class GameController : MonoBehaviour, IMessageListener<LevelCompleted>
    {
        [SerializeField] private string _nextLevel;
        private AudioSource _audioSource;
        
        private void Awake()
        {
            Assert.IsFalse(string.IsNullOrEmpty(_nextLevel), $"Specify the next level in {nameof(GameController)} on {gameObject.name} Game Object");
            InitializeAudio();

            void InitializeAudio()
            {
                _audioSource = new GameObject("Audio").AddComponent<AudioSource>();
            }
            
            Messenger.Subscribe(this);
        }

        private void OnDestroy() => Messenger.Unsubscribe(this);

        private void ToggleSound()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
            else
            {
                _audioSource.Play();
            }
        }

        public void OnMessage(LevelCompleted message) => SceneManager.LoadScene(_nextLevel);
    }
}