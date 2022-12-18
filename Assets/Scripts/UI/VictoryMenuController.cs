using Core.MessageSystem;
using Messages;
using UnityEngine;
using UnityEngine.UI;

public sealed class VictoryMenuController : MonoBehaviour
{
    [SerializeField] private Image[] _starsImages;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private Button _skipButton;
    [Space(5)] [SerializeField] private CanvasGroup _canvasGroup;

    private void Awake() => _skipButton.onClick.AddListener(SkipButtonClicked);

    private void SkipButtonClicked()
    {
        _canvasGroup.blocksRaycasts = false;
        Messenger.Send(new OpenNextLevel());
    }
}
