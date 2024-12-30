using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UISaveGoalScene : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private TMP_Text eventName;
    [SerializeField] private TMP_Text eventDescription;
    [SerializeField] private Image eventIcon;

    private SaveGoalEvents myEvent;
    private GameLoopManager myGameLoopManager;

    private void Awake()
    {
        EventsManager.OnUserClickedAtScreen += OnUserClickedAtScreen;
        EventsManager.OnShowGoalSceneEventScreen += OnShowGoalSceneEventScreen;

    }

    private void OnDestroy()
    {
        EventsManager.OnUserClickedAtScreen -= OnUserClickedAtScreen;
        EventsManager.OnShowGoalSceneEventScreen -= OnShowGoalSceneEventScreen;
    }

    private void OnShowGoalSceneEventScreen(SaveGoalEvents saveGoalEvent, GameLoopManager gameLoopManager)
    {
        myEvent = saveGoalEvent;
        myGameLoopManager = gameLoopManager;

        eventName.text = saveGoalEvent.eventName;
        eventDescription.text = saveGoalEvent.eventDescription;
        eventIcon.sprite = saveGoalEvent.eventIcon;
        EventsManager.OnShowScreen?.Invoke(myPanel.name);
    }

    void OnUserClickedAtScreen()
    {
        if(myPanel.activeSelf)
            myEvent.SaveGoalEventMethod(myGameLoopManager);
    }
    

}
