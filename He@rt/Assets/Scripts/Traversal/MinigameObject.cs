using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameObject : MonoBehaviour {

    private int myID;

    public void SetMyID(int id)
    { myID = id; }

    private void OnTriggerEnter(Collider other)
    {
        if (CharacterSetUp.Game.myID == TraversalManager.CurrentPartyLeader && other.gameObject.tag == "Player")
            GetComponent<PhotonView>().RPC("RPC_StartMinigame", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_StartMinigame()
    { GameObject.Find("Manager").GetComponent<TraversalManager>().InitiateMinigame(myID); }
}
