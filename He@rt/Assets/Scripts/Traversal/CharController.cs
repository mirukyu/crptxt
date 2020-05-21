using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 4f;
    private PhotonView PV;

    Vector3 forward, right;
    // Use this for initialization
    void Start()
    {
        PV = GetComponent<PhotonView>();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (TraversalManager.MoveAllowed && CharacterSetUp.Game.myID == TraversalManager.CurrentPartyLeader)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            { PV.RPC("RPC_PlayWalkingSound", RpcTarget.All); Move(); }
        }
    }
    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        PV.RPC("RPC_Move", RpcTarget.All, heading, rightMovement, upMovement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //collision.gameObject.GetComponent<>(TraversalManager);
    }

    /// RPC

    [PunRPC]
    private void RPC_PlayWalkingSound()
    { GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("Footsteps", ""); }

    [PunRPC]
    private void RPC_Move(Vector3 heading, Vector3 rightMovement, Vector3 upMovement)
    {
        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
    }
}
