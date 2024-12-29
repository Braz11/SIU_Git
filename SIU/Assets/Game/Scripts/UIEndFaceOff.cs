using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum ActionBtn
{
    Pass,
    Shoot,
    Dribble,
    LongPass
}
public class UIEndFaceOff : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private TMP_Text teamWinText;

    [SerializeField] private Button passBtn;
    [SerializeField] private Button shootBtn;
    [SerializeField] private Button dribleBtn;
    [SerializeField] private Button longPassBtn;

    private CTeam.Player lastPlayerThatPlayed;

    private void Awake() {
        EventsManager.OnShowEndFaceoffScreen += OnShowEndFaceoffScreen;

        passBtn.onClick.AddListener(() => { ActionBtnClicked(ActionBtn.Pass); });
        shootBtn.onClick.AddListener(() => { ActionBtnClicked(ActionBtn.Shoot); });
        dribleBtn.onClick.AddListener(() => { ActionBtnClicked(ActionBtn.Dribble); });
        longPassBtn.onClick.AddListener(() => { ActionBtnClicked(ActionBtn.LongPass); });
    }
    private void OnDestroy() {
        EventsManager.OnShowEndFaceoffScreen -= OnShowEndFaceoffScreen;
    }

    private void OnShowEndFaceoffScreen(EndFaceoffData data)
    {
        EventsManager.OnShowScreen?.Invoke(myPanel.name);
        if (data.teamNumber == 1) {
            teamWinText.text = "<color=blue>BLUE</color> wins!";
        } else {
            teamWinText.text = "<color=red>RED</color> wins!";
        }

        lastPlayerThatPlayed = data.playerThatPlayed;

        Debug.Log("Player that played: " + data.playerThatPlayed.name);

        HideAllBtns();

        switch (data.playerThatPlayed.position)
        {
            case CTeam.PlayerPositions.CB:
                passBtn.gameObject.SetActive(true);
                longPassBtn.gameObject.SetActive(true);
                break;
            case CTeam.PlayerPositions.MC:
                passBtn.gameObject.SetActive(true);
                dribleBtn.gameObject.SetActive(true);
                break;
            case CTeam.PlayerPositions.ST:
                shootBtn.gameObject.SetActive(true);
                break;
        }
        
    }

   private void ActionBtnClicked(ActionBtn action)
   {

        switch(lastPlayerThatPlayed.position)
        {
            case CTeam.PlayerPositions.CB:
                if(action == ActionBtn.Pass)
                    EventsManager.OnRandomEventChance?.Invoke(action, lastPlayerThatPlayed, CTeam.PlayerPositions.MC);
                else if(action == ActionBtn.LongPass)
                    EventsManager.OnRandomEventChance?.Invoke(action, lastPlayerThatPlayed, CTeam.PlayerPositions.ST);
                break;
            case CTeam.PlayerPositions.MC:
                if(action == ActionBtn.Pass)
                    EventsManager.OnRandomEventChance?.Invoke(action, lastPlayerThatPlayed, CTeam.PlayerPositions.ST);
                else if(action == ActionBtn.Dribble)
                    EventsManager.OnRandomEventChance?.Invoke(action, lastPlayerThatPlayed, CTeam.PlayerPositions.MC);
                break;
            case CTeam.PlayerPositions.ST:
                if(action == ActionBtn.Shoot)
                    EventsManager.OnRandomEventChance?.Invoke(action, lastPlayerThatPlayed, CTeam.PlayerPositions.ST);
                break;
        }
   }

   private void HideAllBtns()
   {
        passBtn.gameObject.SetActive(false);
        shootBtn.gameObject.SetActive(false);
        dribleBtn.gameObject.SetActive(false);
        longPassBtn.gameObject.SetActive(false);
   }
}
