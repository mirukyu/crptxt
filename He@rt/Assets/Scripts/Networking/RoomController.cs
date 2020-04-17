using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// Needs to be placed in the Room Creation Scene
public class RoomController : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room Info
    public static RoomController room;

    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    //Player Info
    Photon.Realtime.Player[] playerList;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;

    //Delayed Start 
    /*
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;
    */

    private void Awake()
    {
        if (RoomController.room == null)
        { RoomController.room = this; }
        else
        {
            if (RoomController.room != this)
            {
                Destroy(RoomController.room.gameObject);
                RoomController.room = this;
            }
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void Start ()
    {
        PV = GetComponent<PhotonView>();
        /*
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
        */
    }

    /*
    void Update()
    {
        if (MultiplayerSetting.multiplyerSetting.delayStart)
        {
            if(playersInRoom == 1)
            { RestartTimer(); }
            if (!isGameLoaded)
            {
                if(readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                Debug.Log("Time until start: " + timeToStart);
                if (timeToStart <= 0)
                { StartGame(); }
            }
        }
    }
    */

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        playerList = PhotonNetwork.PlayerList;
        playersInRoom = playerList.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = GetComponent<MainMenuManager>().GetNetworkName();

        PV.RPC("RPC_UpdateScreen", RpcTarget.All);

        /*
        if (MultiplayerSetting.multiplyerSetting.delayStart)
        {
            if (playersInRoom > 1)
            { readyToCount = true; }
            if (playersInRoom == MultiplayerSetting.multiplyerSetting.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false; // Room Privacy is set to Private
            }
        }
        else
        {
            StartGame();
        }
        */
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        playerList = PhotonNetwork.PlayerList;
        playersInRoom++;

        /*
        if (MultiplayerSetting.multiplyerSetting.delayStart)
        {
            if (playersInRoom > 1)
            { readyToCount = true; }
            if (playersInRoom == MultiplayerSetting.multiplyerSetting.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false; // Room Privacy is set to Private
            }
        }
        */
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //PV.RPC("RPC_UpdateScreen", RpcTarget.Others);
        base.OnPlayerLeftRoom(otherPlayer);
        playerList = PhotonNetwork.PlayerList;
        playersInRoom--;
        PV.RPC("RPC_UpdateScreen", RpcTarget.All);
    }

    /*
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        GetComponent<MainMenuManager>().IsMaster();
    }
    //*/

    public void StartGame() // to load into the multiplayer scene
    {
        isGameLoaded = true;

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (MultiplayerSetting.multiplyerSetting.delayStart)
        { PhotonNetwork.CurrentRoom.IsOpen = false; }

        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplyerSetting.multiplayerScene);
    }

    void RestartTimer()
    {
        /*
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
        */
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSetting.multiplyerSetting.multiplayerScene)
        {
            isGameLoaded = true;
            if(MultiplayerSetting.multiplyerSetting.delayStart)
            {  PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient); }
            else
            { RPC_CreatePlayer(); }
        }
    }

    [PunRPC]
    private void RPC_UpdateScreen()
    {
        GetComponent<MainMenuManager>().IsMaster(false);
        if (PhotonNetwork.IsMasterClient)
        { GetComponent<MainMenuManager>().IsMaster(true); }
        GetComponent<MainMenuManager>().RoomEnter(PhotonNetwork.CurrentRoom.PlayerCount + " / " + MultiplayerSetting.multiplyerSetting.maxPlayers + " Players", PhotonNetwork.CurrentRoom.Name, PhotonNetwork.PlayerList);
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length)
        { PV.RPC("RPC_CreatePlayer", RpcTarget.All); }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    { /*PhotonNetwork.Instantiate("Test", Vector3.zero, Quaternion.identity, 0);*/ }
}
