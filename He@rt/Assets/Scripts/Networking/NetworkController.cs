using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needs to be placed in the Room Creation Scene
public class NetworkController : MonoBehaviourPunCallbacks
{
    public static NetworkController lobby;

    private void Awake()
    {
        lobby = this; // Creates the singleton, lives within the Main menu scene
    }

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connects to Master Photon Server
    }

    public override void OnConnectedToMaster() // for very first connection
    {
        PhotonNetwork.AutomaticallySyncScene = true; // The Master's screen is proagated
    }
}
