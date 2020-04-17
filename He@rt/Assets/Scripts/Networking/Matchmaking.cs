using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needs to be placed in the Room Creation Scene
public class Matchmaking : MonoBehaviourPunCallbacks {

    // Joining
    public void JoinRandomRoom()
    { PhotonNetwork.JoinRandomRoom(); }

    public void JoinCustomRoom(string RoomName)
    { PhotonNetwork.JoinRoom(RoomName); }

    //Joining Failed - Automatic Calling
    public override void OnJoinRandomFailed(short returnCode, string message)
    { CreateRandomRoom(); }

    public override void OnJoinRoomFailed(short returnCode, string message)
    { GetComponent<MainMenuManager>().JoiningCustomFailed(); }

    // Creating Rooms
    public void CreateRandomRoom()
    {
        int RandoRoomNum = Random.Range(0, 10000);
        RoomOptions roomOpts = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplyerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room " + RandoRoomNum, roomOpts);
    }

    public void CreateCustomRoom(string RoomName)
    {
        RoomOptions roomOpts = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplyerSetting.maxPlayers };
        PhotonNetwork.CreateRoom(RoomName, roomOpts);
    }

    //Creating Failed - Automatic Calling
    public override void OnCreateRoomFailed(short returnCode, string message)
    { CreateRandomRoom(); }

    // Joining or Room Creation Success
    // They are in RoomController

    // Cancelling
    public void Cancel()
    {
        PhotonNetwork.LeaveRoom();
    }
}
