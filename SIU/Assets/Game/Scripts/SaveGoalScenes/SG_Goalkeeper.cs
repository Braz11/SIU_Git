using UnityEngine;

[CreateAssetMenu(fileName = "Goalkeeper", menuName = "Goal Saved/Goalkeeper")]
public class SG_Goalkeeper : SaveGoalEvents
{
    public override void SaveGoalEventMethod(GameLoopManager gameManager)
    {
        if(gameManager.CurrentTeamWinning.teamColor == TeamColor.Blue)
            EventsManager.OnThrowToMidfield?.Invoke(gameManager.GetTeamFromID(2));
        else
            EventsManager.OnThrowToMidfield?.Invoke(gameManager.GetTeamFromID(1));
    }
    
}
