using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TraversalManager : MonoBehaviour {

    public static bool MoveAllowed;

    private static int LastScene = 0; // 0: CharacterSelection ; 1 : Battle Arena ; 2 : Battle Arena (Boss) ; 3 : Dive Into Heck ; 4 : KFC
    public static bool BossBattle = false;
    public static bool JustFled = true;

    public GameObject LoadIcon;
    public Text ResultRecap;
    public Text LeaderName;
    public GameObject MinigameImage;
    public GameObject NormalBattleImage;
    public GameObject BossBattleImage;

    public GameObject YouAreThePartyLeaderText;

    private List<Entity> EntityList = new List<Entity> { CharacterSetUp.Player1, CharacterSetUp.Player2, CharacterSetUp.Player3, CharacterSetUp.Player4 };
    private PhotonView PV;

    public static int CurrentPartyLeader = -1;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private Transform InitialPosition;
    private static Vector3 LastPosition = Vector3.zero;
    private static Quaternion LastRotation = Quaternion.identity;

    [SerializeField] private GameObject AramushaRoot;
    [SerializeField] private GameObject PriestessRoot;
    [SerializeField] private GameObject MageRoot;
    [SerializeField] private GameObject CrusaderRoot;
    private GameObject Root2Use;
    private GameObject PlayerModel;

    public static Entity Enemy1;
    public static Entity Enemy2;
    public static Entity Enemy3;
    public static Entity Enemy4;

    EntityType[] PossibleEnemyTypes = { EntityType.JackOLantern, EntityType.Skelly, EntityType.WispBlue, EntityType.WispRed };

    private static GameObject[] EnemiesList;
    private static List<List<Entity>> EnemiesForEachInstance = new List<List<Entity>>();

    private static GameObject[] MiniGameObjectList;
    private static List<bool> MiniGameObjectActivatedList = new List<bool>();

    public void Awake()
    {
        EnemiesList = GameObject.FindGameObjectsWithTag("Enemy");
        MiniGameObjectList = GameObject.FindGameObjectsWithTag("MiniGame");
        SetIDs();

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < EnemiesList.Length; i++)
                GetComponent<PhotonView>().RPC("RPC_SetEnemyTypes", RpcTarget.All, Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length), 
                    Random.Range(0, PossibleEnemyTypes.Length), Random.Range(0, PossibleEnemyTypes.Length), i);

            for (int i = 0; i < MiniGameObjectList.Length; i++)
                MiniGameObjectActivatedList.Add(false);
        }
    }

    public void SetIDs()
    {
        for (int i = 0; i < EnemiesList.Length; i++)
        { EnemiesList[i].GetComponent<EnemyTraversal>().SetMyID(i); }

        for (int i = 0; i < MiniGameObjectList.Length; i++)
        { MiniGameObjectList[i].GetComponent<MinigameObject>().SetMyID(i); }
    }

    public void SetEnemies()
    {
        for (int i = 0; i < EnemiesList.Length; i++)
        {
            if (EnemiesList[i] != null)
                EnemiesList[i].GetComponent<EnemyTraversal>().SetMyEnemies(EnemiesForEachInstance[i]);
        }
    }

    public void SetMiniGames()
    {
        for (int i = 0; i < MiniGameObjectList.Length; i++)
        {
            if (MiniGameObjectActivatedList[i] == true)
                Destroy(MiniGameObjectList[i]);
        }
    }

    [PunRPC]
    private void RPC_SetEnemyTypes(int type1, int type2, int type3, int type4, int myID)
    {
        Entity tmpEnemy1;
        Entity tmpEnemy2;
        Entity tmpEnemy3;
        Entity tmpEnemy4;

        if (EnemiesList[myID].GetComponent<EnemyTraversal>().IsBoss)
            tmpEnemy1 = EntityCreation.NPCCreation(EntityType.BunBun, 4);
        else
            tmpEnemy1 = EntityCreation.NPCCreation(PossibleEnemyTypes[type1], 4);

        if (CharacterSetUp.Player2 != null)
            tmpEnemy2 = EntityCreation.NPCCreation(PossibleEnemyTypes[type2], 5);
        else
            tmpEnemy2 = null;

        if (CharacterSetUp.Player3 != null)
            tmpEnemy3 = EntityCreation.NPCCreation(PossibleEnemyTypes[type3], 6);
        else
            tmpEnemy3 = null;

        if (CharacterSetUp.Player4 != null)
            tmpEnemy4 = EntityCreation.NPCCreation(PossibleEnemyTypes[type4], 7);
        else
            tmpEnemy4 = null;

        List<Entity> newList = new List<Entity> { tmpEnemy1, tmpEnemy2, tmpEnemy3, tmpEnemy4 };
        EnemiesForEachInstance.Add(newList);
    }

    public void Start()
    {
        SetEnemies();
        SetMiniGames();

        MoveAllowed = true;

        for (int i = 0; i < EnemiesList.Length; i++)
        { EnemiesList[i].GetComponent<EnemyTraversal>().SetMyEnemies(EnemiesForEachInstance[i]); }

        NextPartyLeader();
        EntityType PartyLeaderType = EntityList[CurrentPartyLeader].Type;

        switch (PartyLeaderType)
        {
            case EntityType.Aramusha:
                Root2Use = AramushaRoot;
                break;
            case EntityType.Priestess:
                Root2Use = PriestessRoot;
                break;
            case EntityType.Mage:
                Root2Use = MageRoot;
                break;
            case EntityType.Crusader:
                Root2Use = CrusaderRoot;
                break;
        }

        PlayerModel = Instantiate(Resources.Load<GameObject>(CharacterMenuManager.GetModel(PartyLeaderType)), Root2Use.transform.position, Root2Use.transform.rotation);
        PlayerModel.transform.parent = Root2Use.transform;

        if (JustFled)
        { LastPosition = InitialPosition.position + new Vector3(0f, 1f, 0f); JustFled = false; }

        Avatar.transform.position = LastPosition;
        Avatar.transform.rotation = LastRotation;

        StartCoroutine(PlayAudio());

        PV = GetComponent<PhotonView>();

        LoadIcon.SetActive(false);
        BossBattleImage.SetActive(false);
        NormalBattleImage.SetActive(false);
        MinigameImage.SetActive(false);

        LoadIcon.SetActive(false);
        ResultRecap.text = "";
        LeaderName.text = "";
        YouAreThePartyLeaderText.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            string LeaderName = EntityList[CurrentPartyLeader].TypeName + " (" + ((Player)EntityList[CurrentPartyLeader]).Username + ")";
            PV.RPC("RPC_SetCurrentLeaderName", RpcTarget.All, LeaderName, CurrentPartyLeader);

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
                PV.RPC("RPC_SetMinigameRecap", RpcTarget.All);
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

    public void NextPartyLeader()
    {
        CurrentPartyLeader += 1;
        if (CurrentPartyLeader >= RoomController.room.playersInRoom)
        { CurrentPartyLeader = 0; }
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
        PV.RPC("RPC_SetInfo", RpcTarget.All, LastScene, Avatar.transform.position, Avatar.transform.rotation);
        StartCoroutine(DelayedLoad(SceneToLoad));
    }

    public IEnumerator DelayedLoad(string SceneToLoad)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneToLoad);
    }

    private IEnumerator YouAreThePartyLeaderPopUp()
    {
        YouAreThePartyLeaderText.SetActive(true);

        float currentTime = 0f;
        float FadeDuration = 3f;
        float alpha = 1f;
        Color myColor = new Color(0.90f, 0.00f, 0.30f, 1f);
        yield return new WaitForSeconds(3f);

        while (currentTime < FadeDuration)
        {
            currentTime += Time.deltaTime;
            alpha = Mathf.Lerp(1f, 0f, currentTime / FadeDuration);
            YouAreThePartyLeaderText.GetComponent<Text>().color = new Color(myColor.r, myColor.g, myColor.b, alpha);
            yield return null;
        }
        YouAreThePartyLeaderText.SetActive(false);
        YouAreThePartyLeaderText.GetComponent<Text>().color = new Color(myColor.r, myColor.g, myColor.b, 1f);
        yield break;
    }

    public void InitiateCombat(bool bossBattle, int EncounteredEnemyID)
    {
        Enemy1 = EnemiesForEachInstance[EncounteredEnemyID][0];
        Enemy2 = EnemiesForEachInstance[EncounteredEnemyID][1];
        Enemy3 = EnemiesForEachInstance[EncounteredEnemyID][2];
        Enemy4 = EnemiesForEachInstance[EncounteredEnemyID][3];

        if (PhotonNetwork.IsMasterClient)
        {
            if (bossBattle)
                LoadBossBattle();
            else
                LoadCombat();
        }
    }

    public void InitiateMinigame(int EncounteredMiniGameObjectID)
    {
        MiniGameObjectActivatedList[EncounteredMiniGameObjectID] = true;

        if (Random.Range(0, 3) == 0)
            LoadKFC();
        else
            LoadDiveIntoHeck();
    }

    /// RPC

    [PunRPC]
    private void RPC_LoadingIconSet()
    {
        MoveAllowed = false;
        StartCoroutine(GetComponent<AudioManager>().FadeOut(3f));
        LoadIcon.SetActive(true);
    }

    [PunRPC]
    private void RPC_SetCurrentLeaderName(string name, int PartyLeaderID)
    {
        LeaderName.text = "The current Party Leader is: <color=#ff0000ff>" + name + "</color>.";

        if (CharacterSetUp.Game.myID == PartyLeaderID)
        {
            StartCoroutine(YouAreThePartyLeaderPopUp());
        }
    }

    [PunRPC]
    private void RPC_SetMinigameRecap()
    { ResultRecap.text += "<i>Minigame Results:</i>"; }

    [PunRPC]
    private void RPC_ApplyResult(int ID, int health, int mana)
    {
        EntityList[ID].HealAndManaRegenFromMinigame(health, mana);
        ResultRecap.text += "\n<color=#800080ff>" + EntityList[ID].TypeName + " (" + ((Player)EntityList[ID]).Username + ")</color> has received <color=#00ff00ff>" + health + "</color> HP and <color=#00ffffff>" + mana + "</color> MP.";
    }

    [PunRPC]
    private void RPC_SetBossBattle(bool state)
    {
        BossBattle = state;
    }

    [PunRPC]
    private void RPC_SetInfo(int i, Vector3 lastPos, Quaternion lastRot)
    {
        switch (i)
        {
            case 1:
                NormalBattleImage.SetActive(true);
                break;
            case 2:
                BossBattleImage.SetActive(true);
                break;
            case 3:
            case 4:
                MinigameImage.SetActive(true);
                break;
        }

        LastPosition = lastPos;
        LastRotation = lastRot;
    }
}
