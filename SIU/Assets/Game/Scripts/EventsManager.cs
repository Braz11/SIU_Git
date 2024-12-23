using System;
using System.Collections.Generic;

public static class EventsManager
{
    public static Action<string> OnTryAddPayer;
    public static Action<string> OnAddedPlayer;
    public static Action<string> OnRemovedPlayer;

    public static Action<string> OnWarningText;
    public static Action<List<CTeam>> OnGameStarted;
    public static Action OnClickedStartGame;

    public static Action<ScrambleData> OnShowScrambleScreen;
    public static Action<FaceOffData> OnShowFaceOffScreen;

    public static Action<ScrambleData> OnStartFaceOff;

    public static Action<string> OnShowScreen;

    public static Action OnUserClickedAtScreen;

    public static Action<PhrasesData> OnPhraseCreated;

    public static Action<int> OnCurrentProgressChanged;

    public static Action<int> OnTeamWin;

}
