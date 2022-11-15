using Core.MessageSystem;
using Messages;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public sealed class MenuController : MonoBehaviour
    {
        public void Show()
        {
            Messenger.Send(new SetInputActiveState {IsActive = false});
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Messenger.Send(new SetInputActiveState {IsActive = true});
            gameObject.SetActive(false);
        }

        public void Exit() => SceneManager.LoadScene(0);
    }
}
