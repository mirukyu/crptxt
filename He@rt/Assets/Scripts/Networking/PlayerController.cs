using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour {

    private PhotonView PV;
    public GameObject myAvatar;

	// Use this for initialization
	void Start () {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(9, GameSetUpController.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
            Transform tmp = GameSetUpController.GS.spawnPoints[spawnPicker];
            //myAvatar = PhotonNetwork.Instantiate("Test", tmp.position, tmp.rotation, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
