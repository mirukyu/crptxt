using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameObject : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (CharacterSetUp.Game.myID == TraversalManager.CurrentPartyLeader && other.gameObject.tag == "Player")
        {
            GetComponent<PhotonView>().RPC("RPC_StartMinigame", RpcTarget.MasterClient);
            Destroy(gameObject);
        }
    }

    [PunRPC]
    private void RPC_StartMinigame()
    { GameObject.Find("Manager").GetComponent<TraversalManager>().InitiateMinigame(); }
}
