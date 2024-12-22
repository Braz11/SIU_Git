using System;
using System.Collections.Generic;

public static class EventsManager
{
    public static Action<string> OnTryAddPayer;
    public static Action<string> OnAddedPlayer;
    public static Action<string> OnRemovedPlayer;

    public static Action<string> OnWarningText;
    public static Action OnStartGame;
    public static Action<List<CTeam>> OnDisplayTeams;
}