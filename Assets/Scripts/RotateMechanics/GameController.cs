using Core.MessageSystem;
using DefaultNamespace;
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

        public void OnMessage(LevelCompleted message)
        {
            SetPlayerPrefsValue();
            SceneManager.LoadScene("Main");
        }

        private void SetPlayerPrefsValue()
        {
            if (!PlayerPrefs.HasKey(RotateConstants.LevelPlayerPrefsKey))
            {
                PlayerPrefs.SetInt(RotateConstants.LevelPlayerPrefsKey,1);
            }
            int level = PlayerPrefs.GetInt(RotateConstants.LevelPlayerPrefsKey);
            PlayerPrefs.SetInt(RotateConstants.LevelPlayerPrefsKey,++level);
        }
    }
}