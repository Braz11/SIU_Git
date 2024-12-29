using UnityEngine;

[CreateAssetMenu(fileName = "GE_SimplePrompt", menuName = "Game Event/GE_SimplePrompt")]
public class GE_SimplePrompt : GameEvent
{
    public override void GameEventMethod(GameLoopManager gameManager)
    {
        //does nothing
        Debug.Log("Simple Prompt");
    }
}
