using UnityEngine;

public class CornerInfo
{
    public CTeam currentTeamWinning;
    public PhrasesData phrase;
    public float currentProgress;
}


[CreateAssetMenu(fileName = "Corner", menuName = "Goal Saved/Corner")]
public class SG_Corner : SaveGoalEvents
{
    public override void SaveGoalEventMethod(GameLoopManager gameManager)
    {
        CornerInfo cornerInfo = new CornerInfo();
        cornerInfo.currentTeamWinning = gameManager.CurrentTeamWinning;
        cornerInfo.phrase = gameManager.GetPhraseDataFromType(PhraseType.Corner);
        cornerInfo.currentProgress = gameManager.currentProgress;

        EventsManager.OnShowCornerEventScreen?.Invoke(cornerInfo);
    }
    
}
