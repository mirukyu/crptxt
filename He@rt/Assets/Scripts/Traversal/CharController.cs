using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharController : MonoBehaviour
{
    private float initialMoveSpeed = 4f;
    private float moveSpeed;
    private float jumpSpeed = 50f;
    private float gravity = -20f;

    private PhotonView PV;
    private CharacterController controller = null;

    Vector3 forward, right;

    // Use this for initialization
    void Start()
    {
        PV = GetComponent<PhotonView>();

        controller = GetComponent<CharacterController>();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (TraversalManager.MoveAllowed && CharacterSetUp.Game.myID == TraversalManager.CurrentPartyLeader)
        //if (TraversalManager.MoveAllowed && PV.IsMine)
        {
            //PV = PhotonView.Get(GetComponent<PhotonView>());

            if (Input.GetKey(KeyCode.LeftShift))
                moveSpeed = 1.5f * initialMoveSpeed;
            else
                moveSpeed = initialMoveSpeed;

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            { PV.RPC("RPC_PlayWalkingSound", RpcTarget.All); }

            Move();
        }
    }

    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        direction *= moveSpeed;

        if (controller.isGrounded == true)
        {
            if (Input.GetButtonDown("Jump") == true)
            { direction.y = jumpSpeed * initialMoveSpeed * Time.deltaTime; }
            else //Input.GetButton("Jump") == false
            { direction.y = -0.1f; }
        }
        else // controller.isGrounded == false
            direction.y += gravity * initialMoveSpeed * Time.deltaTime;

        //
        if (heading != Vector3.zero)
            transform.forward = heading;
        controller.Move(direction * Time.deltaTime);
        //PV.RPC("RPC_Move", RpcTarget.All, heading, direction);
    }

    /// RPC

    [PunRPC]
    private void RPC_PlayWalkingSound()
    { GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("Footsteps", ""); }

    [PunRPC]
    private void RPC_Move(Vector3 heading, Vector3 direction)
    {
        if (heading != Vector3.zero)
            transform.forward = heading;
        controller.Move(direction * Time.deltaTime);
    }
}
