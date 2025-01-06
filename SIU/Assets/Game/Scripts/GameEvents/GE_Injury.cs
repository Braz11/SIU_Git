using UnityEngine;

[CreateAssetMenu(fileName = "Injury", menuName = "Game Event/Injury")]
public class GE_Injury : GameEvent
{
    public override void GameEventMethod(GameLoopManager gameManager)
    {
        Debug.Log("Injury");
    }
    
}
