using UnityEngine;

[CreateAssetMenu(fileName = "Offside", menuName = "Game Event/Offside")]
public class GE_Offside : GameEvent
{
    public override void GameEventMethod(GameLoopManager gameManager)
    {
        Debug.Log("Offside");
    }
    
}
