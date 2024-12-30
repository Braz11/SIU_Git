using System.Collections.Generic;
using Unity.VisualScripting;
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

    public ScrambleData scrambleData;

    public PhrasesData phrase;
}
public struct EndFaceoffData
{
    public CTeam.Player playerThatPlayed;
    public int teamNumber;
}
public struct MatchupData
{
    public ActionBtn lastActionPreformed;
    public CTeam.Player lastPlayerThatPlayed;
    public CTeam.PlayerPositions nextPositionToBePlayed;
}


public class GameLoopManager : MonoBehaviour
{
    private List<CTeam> teams = new List<CTeam>();

    private List<PhrasesData> phrases = new List<PhrasesData>();

    [SerializeField] private int _currentProgress;

    [SerializeField] private int randomEventChance = 70;
    [Range(0f, 1f)]
    [SerializeField] private float randomSaveEventChance = .2f;

    [SerializeField] private GameEvent[] randomEvents;
    [SerializeField] private SaveGoalEvents[] saveGoalEvents;

    private CTeam currentTeamWinning;
    public CTeam CurrentTeamWinning { get { return currentTeamWinning; } }

    [SerializeField] private MatchupData matchupData;
    public int currentProgress 
    {
        get { return _currentProgress; }
        set { _currentProgress = Mathf.Clamp(value, -3, 3); }
    }
    

    private void Start() {
        EventsManager.OnGameStarted += OnGameStarted;
        EventsManager.OnStartFaceOff += OnStartFaceOff;
        EventsManager.OnPhraseCreated += OnPhraseCreated;
        EventsManager.OnTeamWin += OnTeamWinFaceoff;
        EventsManager.OnDefineNewMatchup += DefineMatchup;
        EventsManager.OnRandomEventChance += CheckForRandomEvent;
        EventsManager.OnCornerGoal += OnCornerGoal;
        EventsManager.OnThrowToMidfield += OnThrowToMidfield;
        EventsManager.OnStartNewGame += OnStartNewGame;
    }

    private void OnDestroy() {
        EventsManager.OnGameStarted -= OnGameStarted;
        EventsManager.OnStartFaceOff -= OnStartFaceOff;
        EventsManager.OnPhraseCreated -= OnPhraseCreated;
        EventsManager.OnTeamWin -= OnTeamWinFaceoff;
        EventsManager.OnDefineNewMatchup -= DefineMatchup;
        EventsManager.OnRandomEventChance -= CheckForRandomEvent;
        EventsManager.OnCornerGoal -= OnCornerGoal;
        EventsManager.OnThrowToMidfield -= OnThrowToMidfield;
        EventsManager.OnStartNewGame -= OnStartNewGame;
    }

    private void OnStartNewGame() {
        StartNewGame();
    }

    private void OnPhraseCreated(PhrasesData data) {
        phrases.Add(data);
    }

    private void OnGameStarted(List<CTeam> teams) {
        this.teams = teams;   
        StartNewGame();
    }
    private void CheckForRandomEvent(ActionBtn lastActionPreformed, CTeam.Player lastPlayerThatPlayed, CTeam.PlayerPositions nextPositionToBePlayed)
    {
        if (Random.Range(0, 100) <= randomEventChance && randomEvents.Length > 0)
        {
            GameEvent gameEvent = randomEvents[Random.Range(0, randomEvents.Length)];
            gameEvent.GameEventMethod(this);

            MatchupData matchupData = new MatchupData();
            matchupData.lastActionPreformed = lastActionPreformed;
            matchupData.lastPlayerThatPlayed = lastPlayerThatPlayed;
            matchupData.nextPositionToBePlayed = nextPositionToBePlayed;

            EventsManager.OnShowGameEventScreen?.Invoke(gameEvent, matchupData);
            Debug.Log("Random Event!");    
        }
        else
        {
            EventsManager.OnDefineNewMatchup?.Invoke(lastActionPreformed, lastPlayerThatPlayed, nextPositionToBePlayed);
        }    
    }

    private void DefineMatchup(ActionBtn lastActionPreformed, CTeam.Player lastPlayerThatPlayed, CTeam.PlayerPositions nextPositionToBePlayed)
    {
        CTeam currentTeamWithPossession = GetTeamNumberFromPlayer(lastPlayerThatPlayed);
        CTeam teamWithoutPossession = teams.Find(team => team != currentTeamWithPossession);

        currentTeamWithPossession.nextPositionPlay = nextPositionToBePlayed;

        
        if(currentTeamWithPossession.teamColor == TeamColor.Blue)
            if(lastActionPreformed == ActionBtn.LongPass)
                currentProgress += 2;
            else
                currentProgress += 1;
        else
            if(lastActionPreformed == ActionBtn.LongPass)
                currentProgress -= 2;
            else
                currentProgress -= 1;

        currentTeamWinning = currentTeamWithPossession;
            
        switch (Mathf.Abs(currentProgress))
        {
            case 0:
                teamWithoutPossession.nextPositionPlay = CTeam.PlayerPositions.MC;
                CreateMatchup("Midfield!");
                break;
            case 1:
                teamWithoutPossession.nextPositionPlay = CTeam.PlayerPositions.CB;
                if(lastActionPreformed == ActionBtn.Dribble)
                    CreateMatchup(currentTeamWithPossession, lastPlayerThatPlayed, "Attack!");
                else
                    CreateMatchup("Attack!");
                break;
            case 2:

                teamWithoutPossession.nextPositionPlay = GetRandomPosition();
                CreateMatchup("Shot!");
                break;
            case 3:
                GoalScored(currentTeamWithPossession);
                break;
        }
    }
    

    //For Normal Matchups
    private void CreateMatchup(string zoneName) {
        ScrambleData data = new ScrambleData();
        data.zoneText = zoneName;
        data.zoneDetailedText = "scramble!";
        data.phraseType = GetRandomPhraseType();

        data.playersTeam1 = GetRandomPlayersFromTeamWithSpecificPosition(1, TeamColor.Blue, teams[0].nextPositionPlay);
        data.playersTeam2 = GetRandomPlayersFromTeamWithSpecificPosition(1, TeamColor.Red, teams[1].nextPositionPlay);

        EventsManager.OnShowScrambleScreen?.Invoke(data);
    }

    //If we have to dribble
    private void CreateMatchup(CTeam winningTeam, CTeam.Player lastPlayerThatPlayed, string zoneName) {
        ScrambleData data = new ScrambleData();
        data.zoneText = zoneName;
        data.zoneDetailedText = "scramble!";
        data.phraseType = GetRandomPhraseType();


        if(winningTeam.teamColor == TeamColor.Blue){
            data.playersTeam1 = new List<CTeam.Player> { lastPlayerThatPlayed };
            data.playersTeam2 = GetRandomPlayersFromTeamWithSpecificPosition(1, TeamColor.Red, teams[1].nextPositionPlay);
    
        }else{
            data.playersTeam1 = GetRandomPlayersFromTeamWithSpecificPosition(1, TeamColor.Blue, teams[0].nextPositionPlay);
            data.playersTeam2 = new List<CTeam.Player> { lastPlayerThatPlayed };
        }
        
        EventsManager.OnShowScrambleScreen?.Invoke(data);
    }


    private void OnStartFaceOff(ScrambleData data) 
    {
        phrases.FindAll(phrase => phrase.phraseType == data.phraseType);
        PhrasesData phrase = phrases[Random.Range(0, phrases.Count)];

        FaceOffData faceOffData = new FaceOffData();
        faceOffData.phrase = phrase;
        faceOffData.scrambleData = data;
        faceOffData.currentProgress = currentProgress;

        EventsManager.OnShowFaceOffScreen?.Invoke(faceOffData);
    }

    private void OnTeamWinFaceoff(FaceOffData data, int teamNumber) {

        EndFaceoffData faceOffData = new EndFaceoffData();

        if(teamNumber == 1){
            faceOffData.playerThatPlayed = data.scrambleData.playersTeam1[0];
        }else{
            faceOffData.playerThatPlayed = data.scrambleData.playersTeam2[0];
        }

        faceOffData.teamNumber = teamNumber;

        if(currentProgress == 2 || currentProgress == -2)
        {
            if(currentTeamWinning != GetTeamFromID(teamNumber))
            {
                if(UnityEngine.Random.value <= randomSaveEventChance)
                {
                    SaveGoalEvents saveGoalEvent = saveGoalEvents[Random.Range(0, saveGoalEvents.Length)];
                    
                    EventsManager.OnShowGoalSceneEventScreen?.Invoke(saveGoalEvent, this);
                }
            }else{
                GoalScored(currentTeamWinning);
            }
        }else{
            EventsManager.OnShowEndFaceoffScreen?.Invoke(faceOffData);
        }
        
    
    }
    
    private void GoalScored(CTeam team)
    {
        EventsManager.OnShowGoalScreen?.Invoke(team);

        EventsManager.OnGoal?.Invoke(team);
    }

    private void StartNewGame()
    {
        currentProgress = 0;

        EventsManager.OnCurrentProgressChanged?.Invoke(currentProgress);

        foreach (CTeam t in teams)
        {
            t.nextPositionPlay = CTeam.PlayerPositions.MC;
        }

        CreateMatchup("Midfield!");
    }

    private void OnCornerGoal(CTeam team)
    {
        GoalScored(team);
    }

    private void OnThrowToMidfield(CTeam team)
    {
        if(team.teamColor == TeamColor.Blue){
            foreach(CTeam t in teams){
                if(t.teamColor == TeamColor.Red)
                    t.nextPositionPlay = CTeam.PlayerPositions.CB;
                else if(t.teamColor == TeamColor.Blue){
                    t.nextPositionPlay = CTeam.PlayerPositions.ST;
                }
            }
            currentProgress = 1;
        }
        else{
            foreach(CTeam t in teams){
                if(t.teamColor == TeamColor.Red)
                    t.nextPositionPlay = CTeam.PlayerPositions.ST;
                else if(t.teamColor == TeamColor.Blue){
                    t.nextPositionPlay = CTeam.PlayerPositions.CB;
                }
            }
            currentProgress = -1;
        }

        CreateMatchup("Attack!");
    }

    #region Utility Methods

    private PhraseType GetRandomPhraseType(){
        return (PhraseType)Random.Range(0, 3);
    }
    private CTeam.PlayerPositions GetRandomPosition(){
        return (CTeam.PlayerPositions)Random.Range(0, 3);
    }

    public PhrasesData GetPhraseDataFromType(PhraseType phraseType)
    {
        return phrases.Find(phrase => phrase.phraseType == phraseType);
    }

    private List<CTeam.Player> GetRandomPlayersFromTeamWithSpecificPosition(int numberOfPlayerToReturn, TeamColor teamColor, CTeam.PlayerPositions position){
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

    private CTeam GetTeamNumberFromPlayer(CTeam.Player player){
        foreach(CTeam team in teams){
            if(team.players.Contains(player)){
                return team;
            }
        }
        return null;
    }

    public CTeam GetTeamFromID(int index)
    {
        return teams[index - 1];
    }
    
    #endregion
}
