using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIGameEventScreen : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private TMP_Text gameEventName;
    [SerializeField] private TMP_Text gameEventDescription;
    [SerializeField] private Image gameEventIcon;
    [SerializeField] private Button continueBtn;
    private MatchupData matchupData;
   private void Awake()
   {
       EventsManager.OnShowGameEventScreen += OnShowGameEventScreen;
       continueBtn.onClick.AddListener(() => { OnContinueButtonPressed(); });
   }
   private void OnDestroy()
   {
        EventsManager.OnShowGameEventScreen -= OnShowGameEventScreen;
        continueBtn.onClick.RemoveListener(() => { OnContinueButtonPressed(); });
   }
   private void OnShowGameEventScreen(GameEvent gameEvent, MatchupData savedMatchupData)
   {
        gameEventName.text = gameEvent.eventName;
        gameEventDescription.text = gameEvent.eventDescription;
        gameEventIcon.sprite = gameEvent.eventIcon;
        matchupData = savedMatchupData;
        EventsManager.OnShowScreen?.Invoke(myPanel.name);
   }
   private void OnContinueButtonPressed()
    {
        EventsManager.OnDefineNewMatchup?.Invoke(matchupData.lastActionPreformed, matchupData.lastPlayerThatPlayed, matchupData.nextPositionToBePlayed);
    }
    

}
