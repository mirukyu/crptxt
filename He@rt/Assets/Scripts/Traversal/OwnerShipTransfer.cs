using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerShipTransfer : MonoBehaviourPun {

    public void ChangeOwner()
    { base.photonView.RequestOwnership(); }

}
