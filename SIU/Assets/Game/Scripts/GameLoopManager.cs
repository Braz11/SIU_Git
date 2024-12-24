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
public struct EndFaceoffData
{
    public int teamNumber;
}


public class GameLoopManager : MonoBehaviour
{
    private List<CTeam> teams = new List<CTeam>();

    private List<PhrasesData> phrases = new List<PhrasesData>();

    [SerializeField]private int _currentProgress;
    private int currentProgress 
    {
        get { return _currentProgress; }
        set { _currentProgress = Mathf.Clamp(value, 0, 6); }
    }
    

    private void Start() {
        EventsManager.OnGameStarted += OnGameStarted;
        EventsManager.OnStartFaceOff += OnStartFaceOff;
        EventsManager.OnPhraseCreated += OnPhraseCreated;
        EventsManager.OnTeamWin += OnTeamWinFaceoff;
        EventsManager.OnDefineNewMatchup += DefineMatchup;
    }

    private void OnDestroy() {
        EventsManager.OnGameStarted -= OnGameStarted;
        EventsManager.OnStartFaceOff -= OnStartFaceOff;
        EventsManager.OnPhraseCreated -= OnPhraseCreated;
        EventsManager.OnTeamWin -= OnTeamWinFaceoff;
        EventsManager.OnDefineNewMatchup -= DefineMatchup;
    }

    private void OnPhraseCreated(PhrasesData data) {
        phrases.Add(data);
    }

    private void OnGameStarted(List<CTeam> teams) {
        this.teams = teams;   

        currentProgress = 3;

        EventsManager.OnCurrentProgressChanged?.Invoke(currentProgress);

        DefineMatchup(CTeam.PlayerPositions.MC);
    }
    private void DefineMatchup(CTeam.PlayerPositions attackingPosition)
    {
        switch (currentProgress)
        {
            case 0:
                GoalScored(teams[0]);
                break;
            case 1:
                CreateMatchup("Shot!" , teams[1], teams[0], attackingPosition, GetRandomPosition());
                break;
            case 2:
                CreateMatchup("Attack", teams[1], teams[0], attackingPosition, CTeam.PlayerPositions.CB);
                break;
            case 3:
                CreateMatchup("Midfield", teams[1], teams[0], attackingPosition, CTeam.PlayerPositions.MC);
                break;
            case 4:
                CreateMatchup("Attack", teams[0], teams[1], attackingPosition, CTeam.PlayerPositions.CB);
                break;
            case 5:
                CreateMatchup("Shot!", teams[0], teams[1], attackingPosition, GetRandomPosition());
                break;
            case 6:
                GoalScored(teams[1]);
                break;
        }
    }

    private void CreateMatchup(string zoneName, CTeam attackingTeam, CTeam defendingTeam, CTeam.PlayerPositions attackingPosition, CTeam.PlayerPositions defendingPosition) {
        ScrambleData data = new ScrambleData();
        data.zoneText = "Attack";
        data.zoneDetailedText = "scramble!";
        data.phraseType = GetRandomPhraseType();
        data.playersTeam1 = GetRandomnPlayersFromTeamWithSpecificPosition(1, attackingTeam.teamColor, CTeam.PlayerPositions.ST);
        data.playersTeam2 = GetRandomnPlayersFromTeamWithSpecificPosition(1, defendingTeam.teamColor, CTeam.PlayerPositions.CB);

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

    private void OnTeamWinFaceoff(int teamNumber) {

        if(teamNumber == 1)
            currentProgress += 1;
        else
            currentProgress -= 1;

        EndFaceoffData faceOffData = new EndFaceoffData();
        faceOffData.teamNumber = teamNumber;
        EventsManager.OnCurrentProgressChanged?.Invoke(currentProgress);
        EventsManager.OnShowEndFaceoffScreen?.Invoke(faceOffData);
    }
    
    private void GoalScored(CTeam team) {
        Debug.Log("Goal scored by " + team.teamName);
        currentProgress = 3;
        DefineMatchup(CTeam.PlayerPositions.MC);
    }

    #region Utility Methods
    
    private PhraseType GetRandomPhraseType(){
        return (PhraseType)Random.Range(0, 3);
    }
    private CTeam.PlayerPositions GetRandomPosition(){
        return (CTeam.PlayerPositions)Random.Range(0, 3);
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
