using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraversal : MonoBehaviour {

    private EntityType Enemy1Type = EntityType.DEFAULT;

    private Entity Enemy1 = null;
    private Entity Enemy2 = null;
    private Entity Enemy3 = null;
    private Entity Enemy4 = null;

    [SerializeField] private GameObject Avatar;

    [SerializeField] private GameObject JackOLanternRoot;
    [SerializeField] private GameObject SkellyRoot;
    [SerializeField] private GameObject WispBlueRoot;
    [SerializeField] private GameObject WispRedRoot;
    [SerializeField] private GameObject BunBunRoot;
    private GameObject Root2Use;
    private GameObject PlayerModel;

    public bool IsBoss = false;

    private int myID;

    public void SetMyID(int id)
    { myID = id; }

    public void SetMyEnemies(List<Entity> myEnemies)
    {
        Enemy1 = myEnemies[0];
        Enemy1Type = myEnemies[0].Type;
        Enemy2 = myEnemies[1];
        Enemy3 = myEnemies[2];
        Enemy4 = myEnemies[3];
        Initiate();
    }

    private void Initiate() // Called After The Enemy Types Have Been Set
    {
        if (!CheckAlive())
        { Destroy(Avatar); }

        switch (Enemy1Type)
        {
            case EntityType.JackOLantern:
                Root2Use = JackOLanternRoot;
                break;
            case EntityType.Skelly:
                Root2Use = SkellyRoot;
                break;
            case EntityType.WispBlue:
                Root2Use = WispBlueRoot;
                break;
            case EntityType.WispRed:
                Root2Use = WispRedRoot;
                break;
            case EntityType.BunBun:
                Root2Use = BunBunRoot;
                break;
        }

        Destroy(PlayerModel);
        PlayerModel = Instantiate(Resources.Load<GameObject>(EntityCreation.GetModel(Enemy1Type)), Root2Use.transform.position, Root2Use.transform.rotation);
        PlayerModel.transform.parent = Root2Use.transform;
    }

    private bool CheckAlive()
    {
        if (Enemy1 != null && !Enemy1.IsKo)
            return true;

        if (Enemy2 != null && !Enemy2.IsKo)
            return true;

        if (Enemy3 != null && !Enemy3.IsKo)
            return true;

        if (Enemy4 != null && !Enemy4.IsKo)
            return true;

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CharacterSetUp.Game.myID == TraversalManager.CurrentPartyLeader && other.gameObject.tag == "Player")
        { GetComponent<PhotonView>().RPC("RPC_StartBattle", RpcTarget.All); }
    }
   
    [PunRPC]
    private void RPC_StartBattle()
    { GameObject.Find("Manager").GetComponent<TraversalManager>().InitiateCombat(IsBoss, myID); }
}
