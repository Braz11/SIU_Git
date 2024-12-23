using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nameTextField;
    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] Button startGameBtn;
    [SerializeField] Button addPlayerBtn;
    [SerializeField] GameObject playerBoxPrefab;
    [SerializeField] Transform content;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject teamDisplay;

    private void Awake() {
        EventsManager.OnWarningText += ShowWarningText;
        EventsManager.OnAddedPlayer += OnAddedPlayer;
        EventsManager.OnStartGame += DisableMainMenu;
        EventsManager.OnDisplayTeams += DisplayTeams;

        addPlayerBtn.onClick.AddListener(() => {
            EventsManager.OnTryAddPayer?.Invoke(nameTextField.text);
            nameTextField.text = "";
        });

        startGameBtn.onClick.AddListener(() => {
            EventsManager.OnStartGame?.Invoke();
        });
    }

    private void OnDestroy() {
        EventsManager.OnWarningText -= ShowWarningText;
        EventsManager.OnAddedPlayer -= OnAddedPlayer;
        EventsManager.OnDisplayTeams += DisplayTeams;
    }

    private void ShowWarningText(string text) {
        warningText.text = text;
    }

    private void OnAddedPlayer(string name) {
        GameObject playerBox = Instantiate(playerBoxPrefab, content);
        playerBox.GetComponent<PlayerBox>().SetName(name);
    }
    private void DisableMainMenu() {
        mainMenu.SetActive(false);
    }
    private void DisplayTeams(List<CTeam> teams)
    {
        teamDisplay.SetActive(true);
        
        foreach(CTeam team in teams) {
            Debug.Log(team.teamName);
            foreach(CTeam.PlayerRole player in team.players)
            {
                Debug.Log(player.name + " " + player.position + " " + player.timesPlayed);
            }
        }
    }
}
