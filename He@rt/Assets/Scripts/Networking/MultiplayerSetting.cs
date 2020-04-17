using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSetting : MonoBehaviour {

    public static MultiplayerSetting multiplyerSetting;

    public bool delayStart;
    // two types of methods to load players into a multiplayer game:
    // 1) continuous load - players can join and leave a game in progress at any time
    // 2) delayed start - all palyers first need to be connected to a room, then the game is loaded

    public int maxPlayers;
    public int menuScene;
    public int multiplayerScene;

    private void Awake()
    {
        if (MultiplayerSetting.multiplyerSetting == null)
        { MultiplayerSetting.multiplyerSetting = this; }
        else
        {
            if (MultiplayerSetting.multiplyerSetting != this)
            { Destroy(this.gameObject); }
        }
        //DontDestroyOnLoad(this.gameObject);
    }
}
