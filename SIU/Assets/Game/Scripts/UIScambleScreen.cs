using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIScambleScreen : UIManager
{   
    [SerializeField] private GameObject myPanel;
    [SerializeField] private TextMeshProUGUI zoneText;
    [SerializeField] private TextMeshProUGUI zoneDetailedText;
    [SerializeField] private TextMeshProUGUI playerNames;

    [SerializeField] private Transform team1Content;
    [SerializeField] private Transform team2Content;

    ScrambleData myData;

    private void Awake() {
        EventsManager.OnShowScrambleScreen += OnShowScrambleScreen;
        EventsManager.OnUserClickedAtScreen += OnUserClickedAtScreen;
    }

    private void OnDestroy() {
        EventsManager.OnShowScrambleScreen -= OnShowScrambleScreen;
        EventsManager.OnUserClickedAtScreen -= OnUserClickedAtScreen;
    }

    private void OnUserClickedAtScreen() {
        if(myPanel.activeSelf) {
            EventsManager.OnStartFaceOff?.Invoke(myData);
        }
    }


    private void OnShowScrambleScreen(ScrambleData data) 
    {
        myData = data;

        EventsManager.OnShowScreen?.Invoke(myPanel.name);

        zoneText.text = data.zoneText;
        zoneDetailedText.text = data.zoneDetailedText;

        data.playersTeam1.ForEach(player => {
            Instantiate(playerNames, team1Content).SetText(player.name);
        });

        data.playersTeam2.ForEach(player => {
            Instantiate(playerNames, team2Content).SetText(player.name);
        });
        
    }
}