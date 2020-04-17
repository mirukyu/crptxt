using Photon.Pun;
using System.IO;
using UnityEngine;

// Needs to be added to any of the Multiplayer Scenes
public class GameSetUpController : MonoBehaviour
{
    public static GameSetUpController GS;
    public Transform[] spawnPoints;

    private void OnEnable()
    {
        if (GameSetUpController.GS == null)
        { GameSetUpController.GS = this; }
    }

    /*
    // Use this for initialization
    void Start()
    {
        CreatePlayer(); //to create a netwrok'ed object; a player for every user that joins
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate("Test", Vector3.zero, Quaternion.identity); // this is from a video : vector xero is position : rotation is QUat
        //PhotonNetwork.Instantiate(null, Vector3.zero, Quaternion.identity);
    }
    */
}
