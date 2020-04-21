using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSetUp : MonoBehaviour {

    public static CharacterSetUp Game;

    public static Entity Player1;
    public static Entity Player2;
    public static Entity Player3;
    public static Entity Player4;

    public static EntityType Enemy1Type; /// All having this (///) will have to be moved into the Traversal Scene, where we will create the enemies just before entering the combat
    public static EntityType Enemy2Type; ///
    public static EntityType Enemy3Type; ///
    public static EntityType Enemy4Type; ///

    EntityType[] PossibleEnemyTypes = { EntityType.JackOLantern, EntityType.Skelly, EntityType.WispBlue, EntityType.WispRed }; ///

    public int myID;

    private void Awake()
    {
        if (CharacterSetUp.Game == null)
        { CharacterSetUp.Game = this; }
        else
        {
            if (CharacterSetUp.Game != this)
            { Destroy(this.gameObject); }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start() ///
    { ///
        if (PhotonNetwork.IsMasterClient) ///
        { ///
            GetComponent<PhotonView>().RPC("RPC_SetEnemyTypes", RpcTarget.All, Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length)); ///
        } ///
    } ///

    public void SetUp()
    {
        Player1 = CharacterCreation(CharacterMenuManager.player1, CharacterMenuManager.player1Name, 0);
        if (CharacterMenuManager.player2Name != null)
            Player2 = CharacterCreation(CharacterMenuManager.player2, CharacterMenuManager.player2Name, 1);
        if (CharacterMenuManager.player3Name != null)
            Player3 = CharacterCreation(CharacterMenuManager.player3, CharacterMenuManager.player3Name, 2);
        if (CharacterMenuManager.player4Name != null)
            Player4 = CharacterCreation(CharacterMenuManager.player4, CharacterMenuManager.player4Name, 3);
    }

    public void SetMyID(int id)
    {
        myID = id;
    }

    public static Entity CharacterCreation(EntityType type, string username, int entityID) // Creates a Player
    {
        Entity tmp = null;
        switch (type)
        {
            case EntityType.Aramusha:
                tmp = new Aramusha(username, entityID);
                break;
            case EntityType.Priestress:
                tmp = new Priestress(username, entityID);
                break;
            case EntityType.Mage:
                tmp = new Mage(username, entityID);
                break;
            case EntityType.Crusader:
                tmp = new Crusader(username, entityID);
                break;
        }

        return tmp;
    }

    [PunRPC] ///
    private void RPC_SetEnemyTypes(int type1, int type2, int type3, int type4) ///
    { ///
        Enemy1Type = PossibleEnemyTypes[type1]; ///
        Enemy2Type = PossibleEnemyTypes[type2]; ///
        Enemy3Type = PossibleEnemyTypes[type3]; ///
        Enemy4Type = PossibleEnemyTypes[type4]; ///
    } ///
}