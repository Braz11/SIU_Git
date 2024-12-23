using JetBrains.Annotations;
using UnityEngine;


public abstract class GameEvent : ScriptableObject
{
    public string eventName;
    public string eventDescription;
    public Sprite eventIcon;
    public abstract void GameEventMethod(GameManager gameManager);
}
