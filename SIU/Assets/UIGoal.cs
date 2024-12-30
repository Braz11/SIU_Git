using TMPro;
using UnityEngine;

public class UIGoal : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] TextMeshProUGUI winsText;

    private void Awake()
    {
        EventsManager.OnUserClickedAtScreen += OnUserClickedAtScreen;
        EventsManager.OnShowGoalScreen += OnShowGoalScreen;
    }
    
    private void OnDestroy()
    {
        EventsManager.OnUserClickedAtScreen -= OnUserClickedAtScreen;
        EventsManager.OnShowGoalScreen -= OnShowGoalScreen;
    }

    private void OnUserClickedAtScreen()
    {
        if(myPanel.activeSelf)
        {
            EventsManager.OnStartNewGame?.Invoke();
        }
    }

    private void OnShowGoalScreen(CTeam winner)
    {
        EventsManager.OnShowScreen?.Invoke(myPanel.name);

        if(winner.teamColor == TeamColor.Blue)
        {
            winsText.text = "<color=blue>BLUE</color> TEAM SIPS IT UP!";
        }
        else
        {
            winsText.text = "<color=red>RED</color> TEAM SIPS IT UP!";
        }

    }
}
