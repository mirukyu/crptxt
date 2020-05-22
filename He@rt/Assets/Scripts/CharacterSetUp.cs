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

    public int myID;

    EntityType[] PossibleEnemyTypes = { EntityType.JackOLantern, EntityType.Skelly, EntityType.WispBlue, EntityType.WispRed };

    public GameObject[] EnemiesList;
    public List<List<Entity>> EnemiesForEachInstance = new List<List<Entity>>();
    public int BossID;

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

    [PunRPC]
    private void RPC_SetEnemyTypes(int type1, int type2, int type3, int type4, int myID)
    {
        Entity tmpEnemy1;
        Entity tmpEnemy2;
        Entity tmpEnemy3;
        Entity tmpEnemy4;

        if (myID == BossID)
            tmpEnemy1 = EntityCreation.NPCCreation(EntityType.BunBun, 4);
        else
            tmpEnemy1 = EntityCreation.NPCCreation(PossibleEnemyTypes[type1], 4);

        if (Player2 != null)
            tmpEnemy2 = EntityCreation.NPCCreation(PossibleEnemyTypes[type2], 5);
        else
            tmpEnemy2 = null;

        if (Player3 != null)
            tmpEnemy3 = EntityCreation.NPCCreation(PossibleEnemyTypes[type3], 6);
        else
            tmpEnemy3 = null;

        if (Player4 != null)
            tmpEnemy4 = EntityCreation.NPCCreation(PossibleEnemyTypes[type4], 7);
        else
            tmpEnemy4 = null;

        List<Entity> newList = new List<Entity> { tmpEnemy1, tmpEnemy2, tmpEnemy3, tmpEnemy4 };
        EnemiesForEachInstance.Add(newList);
    }

    [PunRPC]
    private void RPC_SetBossID(int bossid)
    { BossID = bossid; }

    public void SetUp()
    {
        Player1 = CharacterCreation(CharacterMenuManager.player1, CharacterMenuManager.player1Name, 0);
        if (CharacterMenuManager.player2Name != null)
            Player2 = CharacterCreation(CharacterMenuManager.player2, CharacterMenuManager.player2Name, 1);
        if (CharacterMenuManager.player3Name != null)
            Player3 = CharacterCreation(CharacterMenuManager.player3, CharacterMenuManager.player3Name, 2);
        if (CharacterMenuManager.player4Name != null)
            Player4 = CharacterCreation(CharacterMenuManager.player4, CharacterMenuManager.player4Name, 3);

        if (PhotonNetwork.IsMasterClient) //Setting Up Enemies
        {
            GetComponent<PhotonView>().RPC("RPC_SetBossID", RpcTarget.All, Random.Range(0, 15));

            for (int i = 0; i < 15; i++)
                GetComponent<PhotonView>().RPC("RPC_SetEnemyTypes", RpcTarget.All, Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length),
                    Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length), i);
        }
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
            case EntityType.Priestess:
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
}