using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class UITeamDisplayScreen : MonoBehaviour
{
    [SerializeField] private GameObject myPanel;
    [SerializeField] private GameObject[] containers;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Sprite[] playerIcons;
    [SerializeField] private Button startGameBtn;
    private List<CTeam> teamsList = new List<CTeam>();

    private void Awake()
    {
        EventsManager.OnShowTeamsDisplayScreen += OnShowTeamsDisplayScreen;
        startGameBtn.onClick.AddListener(() => {
            EventsManager.OnGameStarted?.Invoke(teamsList);
            });
    }
    private void OnDestroy()
    {
        EventsManager.OnShowTeamsDisplayScreen -= OnShowTeamsDisplayScreen;
    }
    private void OnShowTeamsDisplayScreen(List<CTeam> teams)
    {
        teamsList = teams;
        EventsManager.OnShowScreen?.Invoke(myPanel.name);

        foreach (GameObject container in containers)
        {
            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }
        }

        int containerIndex = 0;
        for (int i = 0; i < teams.Count; i++)
        {
            containerIndex = FillContainer(containerIndex, CTeam.PlayerPositions.CB, teams[i]);
            containerIndex = FillContainer(containerIndex, CTeam.PlayerPositions.MC, teams[i]);
            containerIndex = FillContainer(containerIndex, CTeam.PlayerPositions.ST, teams[i]);
        }
    }
    private int FillContainer(int index, CTeam.PlayerPositions position, CTeam team)
    {
        
        List<CTeam.Player> players = team.players.FindAll(player => player.position == position);
        foreach (CTeam.Player player in players)
        {
            GameObject playerDisplay = Instantiate(playerPrefab, containers[index].transform);
            playerDisplay.GetComponentInChildren<HeadItem>().Initalize(player.bodyPartInfo);
            playerDisplay.GetComponentInChildren<TextMeshProUGUI>().text = player.name;
            if (playerIcons.Length > 0)
            {
                playerDisplay.GetComponentInChildren<Image>().sprite = playerIcons[Random.Range(0, playerIcons.Length)];
            }

            if(index == 0)
            {
                playerDisplay.GetComponentInChildren<HeadItem>().gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        int newIndex = index + 1;
        return newIndex;
    }
}
