using UnityEngine;

public abstract class SaveGoalEvents : ScriptableObject
{
    public string eventName;
    public string eventDescription;
    public Sprite eventIcon;
    public abstract void SaveGoalEventMethod(GameLoopManager gameManager);
}