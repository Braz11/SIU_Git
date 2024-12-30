using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICornerEvent : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI phraseIdentifier;
    [SerializeField] private TextMeshProUGUI phraseText;
    [SerializeField] private Button team1WinsBtn;
    [SerializeField] private Button team2WinsBtn;

    private CornerInfo myData;

    private void Awake() {
        EventsManager.OnShowCornerEventScreen += OnShowCornerEventScreen;
        EventsManager.OnCurrentProgressChanged += (progress) => progressSlider.value = progress;
        team1WinsBtn.onClick.AddListener(OnTeam1Wins);
        team2WinsBtn.onClick.AddListener(OnTeam2Wins);
    }

    private void OnDestroy() {
        EventsManager.OnShowCornerEventScreen -= OnShowCornerEventScreen;
        EventsManager.OnCurrentProgressChanged -= (progress) => progressSlider.value = progress;
        
        team1WinsBtn.onClick.RemoveListener(OnTeam1Wins);
        team2WinsBtn.onClick.RemoveListener(OnTeam2Wins);
    }

    private void OnShowCornerEventScreen(CornerInfo data) {
        EventsManager.OnShowScreen?.Invoke(myPanel.name);
        
        myData = data;

        progressSlider.value = data.currentProgress;
        phraseIdentifier.text = data.phrase.phraseIdentifier;
        phraseText.text = data.phrase.phrase;
    }
    
    private void OnTeam1Wins() {
        if(myData.currentTeamWinning.teamColor == TeamColor.Blue) {
           EventsManager.OnCornerGoal?.Invoke(myData.currentTeamWinning); 
        }else{
            EventsManager.OnThrowToMidfield?.Invoke(myData.currentTeamWinning);
        }
    }

    private void OnTeam2Wins() {
        if(myData.currentTeamWinning.teamColor == TeamColor.Red) {
           EventsManager.OnCornerGoal?.Invoke(myData.currentTeamWinning); 
        }else{
            EventsManager.OnThrowToMidfield?.Invoke(myData.currentTeamWinning);
        }    
    }
}

