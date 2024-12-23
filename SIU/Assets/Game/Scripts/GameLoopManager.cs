using System.Collections.Generic;
using UnityEngine;

public enum Zone
{
    Attack,
    Midfield,
    Defence
}

public struct ScrambleData
{
    public string zoneText;
    public string zoneDetailedText;

    public PhraseType phraseType;

    public List<CTeam.Player> playersTeam1;
    public List<CTeam.Player> playersTeam2; 
}

public struct FaceOffData
{
    public int currentProgress;

    public PhrasesData phrase;
}


public class GameLoopManager : MonoBehaviour
{
    private List<CTeam> teams = new List<CTeam>();

    private List<PhrasesData> phrases = new List<PhrasesData>();

    private int currentProgress = 0;

    private void Start() {
        EventsManager.OnGameStarted += OnGameStarted;
        EventsManager.OnStartFaceOff += OnStartFaceOff;
        EventsManager.OnPhraseCreated += OnPhraseCreated;
        EventsManager.OnTeamWin += OnTeamWin;
    }

    private void OnDestroy() {
        EventsManager.OnGameStarted -= OnGameStarted;
        EventsManager.OnStartFaceOff -= OnStartFaceOff;
        EventsManager.OnPhraseCreated -= OnPhraseCreated;
        EventsManager.OnTeamWin -= OnTeamWin;
    }

    private void OnPhraseCreated(PhrasesData data) {
        phrases.Add(data);
    }

    private void OnGameStarted(List<CTeam> teams) {
        this.teams = teams;

        foreach(CTeam team in teams) {
            currentProgress += team.teamProgress;
        }

        currentProgress = currentProgress / teams.Count;

        EventsManager.OnCurrentProgressChanged?.Invoke(currentProgress);

        ScrambleData data = new ScrambleData();
        data.zoneText = "Midfield";
        data.zoneDetailedText = "scramble!";
        data.phraseType = GetRandomPhraseType();
        data.playersTeam1 = GetRandomnPlayersFromTeamWithSpecificPosition(1, TeamColor.Red, CTeam.PlayerPositions.MC);
        data.playersTeam2 = GetRandomnPlayersFromTeamWithSpecificPosition(1, TeamColor.Blue, CTeam.PlayerPositions.MC);

        EventsManager.OnShowScrambleScreen?.Invoke(data);
    }

    private void OnStartFaceOff(ScrambleData data) 
    {
        phrases.FindAll(phrase => phrase.phraseType == data.phraseType);
        PhrasesData phrase = phrases[Random.Range(0, phrases.Count)];

        FaceOffData faceOffData = new FaceOffData();
        faceOffData.phrase = phrase;
        faceOffData.currentProgress = currentProgress;

        EventsManager.OnShowFaceOffScreen?.Invoke(faceOffData);
    }

    private void OnTeamWin(int teamNumber) {
        CTeam team = teams[teamNumber - 1];

        if(teamNumber == 1)
            team.teamProgress += 1;
        else
            team.teamProgress -= 1;

        currentProgress = team.teamProgress;

        EventsManager.OnCurrentProgressChanged?.Invoke(currentProgress);
    }

    #region Utiliy Methods
    
    private PhraseType GetRandomPhraseType(){
        return (PhraseType)Random.Range(0, 3);
    }

    private List<CTeam.Player> GetRandomnPlayersFromTeamWithSpecificPosition(int numberOfPlayerToReturn, TeamColor teamColor, CTeam.PlayerPositions position){
        List<CTeam.Player> playersWithPosition = teams.Find(team => team.teamColor == teamColor).players.FindAll(player => player.position == position);
        
        List<CTeam.Player> playersToReturn = new List<CTeam.Player>();

        if(playersWithPosition.Count < numberOfPlayerToReturn){
            return playersWithPosition;
        }   

        for(int i = 0; i < numberOfPlayerToReturn; i++){
            CTeam.Player player = playersWithPosition[Random.Range(0, playersWithPosition.Count)];
            playersToReturn.Add(player);
        }

        return playersToReturn;
    }
    
    #endregion
}
