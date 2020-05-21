using Photon.Pun;
using UnityEngine;

public class FollowObj : MonoBehaviour {
    public Transform target;
    private float Initialspeed = 0.125f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (TraversalManager.MoveAllowed && CharacterSetUp.Game.myID == TraversalManager.CurrentPartyLeader)
        {
            Vector3 adaptedOffset = offset;

            if (Input.GetAxis("Vertical") < 0)
            { adaptedOffset += new Vector3(0, 0, -1.5f); }
            if (Input.GetAxis("Horizontal") != 0)
            { adaptedOffset += new Vector3(0, 0, -0.5f); }

            Vector3 campos = target.position + adaptedOffset;

            float speed = Initialspeed;
            if (Vector3.Distance(transform.position, campos) < 1.5f)
            { speed = Mathf.Lerp(Initialspeed, 0f, Initialspeed); }

            Vector3 smoothing = Vector3.Lerp(transform.position, campos, speed);

            PV.RPC("RPC_CameraMovement", RpcTarget.All, smoothing);
            //transform.position = smoothing;
        }
        //transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, 0.1f);
        //transform.position = smoothing;
        //if we need to make a the cam look at the object we might need new player controller
        //transform.LookAt(target);
    }

    [PunRPC]
    private void RPC_CameraMovement(Vector3 campos)
    { transform.position = campos; }
}
