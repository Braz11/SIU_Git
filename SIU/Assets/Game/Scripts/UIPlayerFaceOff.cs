using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerFaceOff : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI phraseIdentifier;
    [SerializeField] private TextMeshProUGUI phraseText;
    [SerializeField] private Button team1WinsBtn;
    [SerializeField] private Button team2WinsBtn;

    private void Awake() {
        EventsManager.OnShowFaceOffScreen += OnShowFaceOffScreen;
        EventsManager.OnCurrentProgressChanged += (progress) => progressSlider.value = progress;
        team1WinsBtn.onClick.AddListener(OnTeam1Wins);
        team2WinsBtn.onClick.AddListener(OnTeam2Wins);
    }

    private void OnDestroy() {
        EventsManager.OnShowFaceOffScreen -= OnShowFaceOffScreen;
        EventsManager.OnCurrentProgressChanged -= (progress) => progressSlider.value = progress;
        
        team1WinsBtn.onClick.RemoveListener(OnTeam1Wins);
        team2WinsBtn.onClick.RemoveListener(OnTeam2Wins);
    }

    private void OnShowFaceOffScreen(FaceOffData data) {
        EventsManager.OnShowScreen?.Invoke(myPanel.name);
        
        progressSlider.value = data.currentProgress;
        phraseIdentifier.text = data.phrase.phraseIdentifier;
        phraseText.text = data.phrase.phrase;
    }
    
    private void OnTeam1Wins() {
        EventsManager.OnTeamWin?.Invoke(1);
    }

    private void OnTeam2Wins() {
        EventsManager.OnTeamWin.Invoke(2);
    }
}

