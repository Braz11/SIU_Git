using System.Collections.Generic;
using UnityEngine;

public enum TeamColor
{
    Red,
    Blue,
    Orange,
    Yellow,
    Green
}

[System.Serializable]
public class CTeam
{ 
    public string teamName;
    public TeamColor teamColor;

    public enum PlayerPositions
    {
        CB,
        MC,
        ST
    }
    
    [System.Serializable]
    public class Player
    {
        public string name;
        public PlayerPositions position;
        public int timesPlayed;
        public BodyPartInfo bodyPartInfo;
    }

    public List<Player> players;
    public PlayerPositions nextPositionPlay;
}

public class GameManager : MonoBehaviour
{
    private List<string> players = new List<string>();
    private List<CTeam> teams = new List<CTeam>();

    private void Awake() {
        EventsManager.OnTryAddPayer += OnTryAddPayer;
        EventsManager.OnRemovedPlayer += OnRemovedPlayer;
        EventsManager.OnClickedStartGame += OnClickedStartGame;
    }

    private void OnDestroy() {
        EventsManager.OnTryAddPayer -= OnTryAddPayer;
        EventsManager.OnRemovedPlayer -= OnRemovedPlayer;
        EventsManager.OnClickedStartGame -= OnClickedStartGame;
    }

    private void OnTryAddPayer(string name) {
        if(players.Contains(name)) {
            EventsManager.OnWarningText?.Invoke("Player already exists");
            return;
        }
        else if(name == "") {
            EventsManager.OnWarningText?.Invoke("Name cannot be empty");
            return;
        }

        players.Add(name);
        EventsManager.OnAddedPlayer?.Invoke(name);
    }

    private void OnRemovedPlayer(string name) {
        players.Remove(name);
    }

    private void OnClickedStartGame() {
        if(players.Count < 1) {
            EventsManager.OnWarningText?.Invoke("Need at least 1 player to start the game");
            return;
        }


        CreateTeams();

        EventsManager.OnShowTeamsDisplayScreen?.Invoke(teams);
        Debug.Log("Game started with " + players.Count + " players");
    }

    private void CreateTeams() {
        
        System.Random rng = new System.Random();
        int n = players.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            string value = players[k];
            players[k] = players[n];
            players[n] = value;
        }

        int halfCount = players.Count / 2;

        CTeam team1 = new CTeam { teamName = "Team 1", teamColor = TeamColor.Blue, players = new List<CTeam.Player>() };
        CTeam team2 = new CTeam { teamName = "Team 2", teamColor = TeamColor.Red, players = new List<CTeam.Player>() };

        CTeam.PlayerPositions[] positions = { CTeam.PlayerPositions.CB, CTeam.PlayerPositions.MC, CTeam.PlayerPositions.ST };

        int[] positionCounts = new int[positions.Length];

        for (int i = 0; i < players.Count; i++) {
            int minPositionIndex = 0;
            for (int j = 1; j < positions.Length; j++) 
            {
                if (positionCounts[j] < positionCounts[minPositionIndex]) {
                    minPositionIndex = j;
                }
            }

            CTeam.Player playerRole = new CTeam.Player { name = players[i], position = positions[minPositionIndex], timesPlayed = 0 };
            EventsManager.OnCreateBody?.Invoke(playerRole);
            
            positionCounts[minPositionIndex]++;

            if (i < halfCount) {
                team1.players.Add(playerRole);
            } else {
                team2.players.Add(playerRole);
            }
        }

        teams.Add(team1);
        teams.Add(team2);

        foreach (var team in teams) {
            Debug.Log("------- Team: " + team.teamName + ", Color: " + team.teamColor + "--------");
            foreach (var player in team.players) {
            Debug.Log("Player: " + player.name + ", Position: " + player.position + ", Times Played: " + player.timesPlayed);
            }
        }
    }
}