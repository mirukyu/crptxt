using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameObject : MonoBehaviour {

    [SerializeField] private GameObject myObject;

    [SerializeField] private GameObject RedTotem;
    [SerializeField] private GameObject BlueTotem;
    private int myID;

    public void SetMyID(int id)
    { myID = id; }

    public void SetMyStats(int id, bool used)
    {
        myID = id;
        if (used)
        { Destroy(myObject); Destroy(this); }
    }

    public void Start()
    {
        if(Random.Range(0,2) == 0)
        { RedTotem.SetActive(true); BlueTotem.SetActive(false); }
        else
        { RedTotem.SetActive(false); BlueTotem.SetActive(true); }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (CharacterSetUp.Game.myID == TraversalManager.CurrentPartyLeader && other.gameObject.tag == "Player")
            GetComponent<PhotonView>().RPC("RPC_StartMinigame", RpcTarget.MasterClient);
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if (CharacterSetUp.Game.myID == TraversalManager.CurrentPartyLeader && collision.gameObject.tag == "Player")
            GetComponent<PhotonView>().RPC("RPC_StartMinigame", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_StartMinigame()
    { GameObject.Find("Manager").GetComponent<TraversalManager>().InitiateMinigame(myID); }
}
