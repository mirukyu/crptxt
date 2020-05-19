using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TraversalManager : MonoBehaviour {

    private static int LastScene = 0; // 0: CharacterSelection ; 1 : Battle Arena ; 2 : Battle Arena (Boss) ; 3 : Dive Into Heck ; 4 : KFC
    public static bool BossBattle = false;

    public GameObject LoadIcon;
    public GameObject ButtonField;
    public Text ResultRecap;

    private List<Entity> EntityList = new List<Entity> { CharacterSetUp.Player1, CharacterSetUp.Player2, CharacterSetUp.Player3, CharacterSetUp.Player4 };
    private PhotonView PV;

    public List<Transform> PositionTestList;
    public Transform TestObject;

    public static EntityType Enemy1Type; // create the enemies just before entering the combat
    public static EntityType Enemy2Type;
    public static EntityType Enemy3Type;
    public static EntityType Enemy4Type;

    EntityType[] PossibleEnemyTypes = { EntityType.JackOLantern, EntityType.Skelly, EntityType.WispBlue, EntityType.WispRed };

    void Start()
    {
        StartCoroutine(PlayAudio());

        PV = GetComponent<PhotonView>();

        LoadIcon.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
            ButtonField.SetActive(true);
        else
            ButtonField.SetActive(false);

        LoadIcon.SetActive(false);
        ResultRecap.text = "Loaded\n";

        if (PhotonNetwork.IsMasterClient)
        {
            TestObject.position = PositionTestList[LastScene].position;

            List<int> Results = null;

            switch (LastScene)
            {
                case 3:
                    Results = DiveIntoHeccManager.Results;
                    break;
                case 4:
                    Results = KFCManager.Results;
                    break;
            }

            if (Results != null)
            {
                foreach (Entity e in EntityList)
                {
                    if (e != null)
                    {
                        int EntityID = e.EntityID;
                        int healthReward = Results[2 * e.EntityID];
                        int manaReward = Results[2 * e.EntityID + 1];
                        PV.RPC("RPC_ApplyResult", RpcTarget.All, EntityID, healthReward, manaReward);
                    }
                }
            }
        }
    }

    public IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<AudioManager>().PlayTheme(ThemeStyle.Traversal);
        StartCoroutine(GetComponent<AudioManager>().FadeIn(3f));
    }

    public void LoadCombat()
    {
        PV.RPC("RPC_SetBossBattle", RpcTarget.All, false);
        LastScene = 1;
        LoadNextScene("Battle Arena");
        GetComponent<PhotonView>().RPC("RPC_SetEnemyTypes", RpcTarget.All, Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length));
    }
    public void LoadBossBattle()
    {
        PV.RPC("RPC_SetBossBattle", RpcTarget.All, true);
        LastScene = 2;
        LoadNextScene("Battle Arena");
    }
    public void LoadDiveIntoHeck()
    {
        LastScene = 3;
        LoadNextScene("Dive Into Heck");
    }
    public void LoadKFC()
    {
        LastScene = 4;
        LoadNextScene("KFC");
    }

    public void LoadNextScene(string SceneToLoad)
    {
        PV.RPC("RPC_LoadingIconSet", RpcTarget.All);
        StartCoroutine(DelayedLoad(SceneToLoad));
        StartCoroutine(GetComponent<AudioManager>().FadeOut(3f));
    }

    public IEnumerator DelayedLoad(string SceneToLoad)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneToLoad);
    }

    [PunRPC]
    private void RPC_LoadingIconSet()
    {
        LoadIcon.SetActive(true);
    }

    [PunRPC]
    private void RPC_ApplyResult(int ID, int health, int mana)
    {
        EntityList[ID].HealAndManaRegenFromMinigame(health, mana);
        ResultRecap.text += EntityList[ID].TypeName + " has received " + health + " Health Points and " + mana + " Mana Points.\n";
    }

    [PunRPC]
    private void RPC_SetEnemyTypes(int type1, int type2, int type3, int type4)
    {
        Enemy1Type = PossibleEnemyTypes[type1];
        if (CharacterSetUp.Player2 != null)
            Enemy2Type = PossibleEnemyTypes[type2];
        if (CharacterSetUp.Player3 != null)
            Enemy3Type = PossibleEnemyTypes[type3];
        if (CharacterSetUp.Player4 != null)
            Enemy4Type = PossibleEnemyTypes[type4];
    }

    [PunRPC]
    private void RPC_SetBossBattle(bool state)
    {
        BossBattle = state;
    }
}
