using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCreation : MonoBehaviour
{

    [SerializeField] private GameObject Player1Overview;
    [SerializeField] private GameObject Player2Overview;
    [SerializeField] private GameObject Player3Overview;
    [SerializeField] private GameObject Player4Overview;

    [SerializeField] private GameObject Player1SpawnPoint;
    [SerializeField] private GameObject Player2SpawnPoint;
    [SerializeField] private GameObject Player3SpawnPoint;
    [SerializeField] private GameObject Player4SpawnPoint;

    public Entity player1 = CharacterSetUp.Player1;
    public Entity player2 = CharacterSetUp.Player2;
    public Entity player3 = CharacterSetUp.Player3;
    public Entity player4 = CharacterSetUp.Player4;

    private GameObject player1Model;
    private GameObject player2Model;
    private GameObject player3Model;
    private GameObject player4Model;

    //////////////////////////////////////////////

    public bool BossBattle; // This boolean switches between Boss Fight and Normal Fight

    [SerializeField] private GameObject NPC1Overview;
    [SerializeField] private GameObject NPC2Overview;
    [SerializeField] private GameObject NPC3Overview;
    [SerializeField] private GameObject NPC4Overview;

    [SerializeField] private GameObject NPC1SpawnPoint;
    [SerializeField] private GameObject NPC2SpawnPoint;
    [SerializeField] private GameObject NPC3SpawnPoint;
    [SerializeField] private GameObject NPC4SpawnPoint;

    [SerializeField] private GameObject BossSpawnPoint;

    public Entity enemy1;
    public Entity enemy2;
    public Entity enemy3;
    public Entity enemy4;

    private GameObject NPC1Model;
    private GameObject NPC2Model;
    private GameObject NPC3Model;
    private GameObject NPC4Model;

    ////////////////////////////////////////

    public List<EntityInfoFieldManager> OverviewList;
    public List<Entity> EntityList;

    // Use this for initialization
    void Start()
    {
        BossBattle = TraversalManager.BossBattle;

        Player2Overview.SetActive(false);
        Player3Overview.SetActive(false);
        Player4Overview.SetActive(false);

        Player1Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((Player)player1).Username, player1);
        player1Model = Instantiate(Resources.Load<GameObject>(CharacterMenuManager.GetModel(player1.Type)), Player1SpawnPoint.transform.position, Player1SpawnPoint.transform.rotation);

        if (player2 != null)
        {
            Player2Overview.SetActive(true);
            Player2Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((Player)player2).Username, player2);
            player2Model = Instantiate(Resources.Load<GameObject>(CharacterMenuManager.GetModel(player2.Type)), Player2SpawnPoint.transform.position, Player2SpawnPoint.transform.rotation);
        }

        if (player3 != null)
        {
            Player3Overview.SetActive(true);
            Player3Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((Player)player3).Username, player3);
            player3Model = Instantiate(Resources.Load<GameObject>(CharacterMenuManager.GetModel(player3.Type)), Player3SpawnPoint.transform.position, Player3SpawnPoint.transform.rotation);
        }

        if (player4 != null)
        {
            Player4Overview.SetActive(true);
            Player4Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((Player)player4).Username, player4);
            player4Model = Instantiate(Resources.Load<GameObject>(CharacterMenuManager.GetModel(player4.Type)), Player4SpawnPoint.transform.position, Player4SpawnPoint.transform.rotation);
        }

        //////////

        BossSpawnPoint.SetActive(false);

        if (BossBattle)
        {
            NPC1SpawnPoint.SetActive(false);
            NPC2SpawnPoint.SetActive(false);
            NPC3SpawnPoint.SetActive(false);
            NPC4SpawnPoint.SetActive(false);
            BossSpawnPoint.SetActive(true);

            enemy1 = TraversalManager.Enemy1;
            //enemy1 = NPCCreation(EntityType.BunBun, 4);
            NPC1SpawnPoint = BossSpawnPoint;
        }
        else
        {
            /*
            enemy1 = NPCCreation(EntityType.JackOLantern, 4);
            enemy2 = NPCCreation(EntityType.Skelly, 5);
            enemy3 = NPCCreation(EntityType.WispBlue, 6);
            enemy4 = NPCCreation(EntityType.WispRed, 7);
            */

            enemy1 = TraversalManager.Enemy1;
            enemy2 = TraversalManager.Enemy2;
            enemy3 = TraversalManager.Enemy3;
            enemy4 = TraversalManager.Enemy4;
        }

        NPC2Overview.SetActive(false);
        NPC3Overview.SetActive(false);
        NPC4Overview.SetActive(false);

        NPC1Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((NPC)enemy1).Name, enemy1);
        NPC1Model = Instantiate(Resources.Load<GameObject>(GetModel(enemy1.Type)), NPC1SpawnPoint.transform.position, NPC1SpawnPoint.transform.rotation);

        if (enemy2 != null)
        {
            NPC2Overview.SetActive(true);
            NPC2Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((NPC)enemy2).Name, enemy2);
            NPC2Model = Instantiate(Resources.Load<GameObject>(GetModel(enemy2.Type)), NPC2SpawnPoint.transform.position, NPC2SpawnPoint.transform.rotation);
        }

        if (enemy3 != null)
        {
            NPC3Overview.SetActive(true);
            NPC3Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((NPC)enemy3).Name, enemy3);
            NPC3Model = Instantiate(Resources.Load<GameObject>(GetModel(enemy3.Type)), NPC3SpawnPoint.transform.position, NPC3SpawnPoint.transform.rotation);
        }

        if (enemy4 != null)
        {
            NPC4Overview.SetActive(true);
            NPC4Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((NPC)enemy4).Name, enemy4);
            NPC4Model = Instantiate(Resources.Load<GameObject>(GetModel(enemy4.Type)), NPC4SpawnPoint.transform.position, NPC4SpawnPoint.transform.rotation);
        }

        if (PhotonNetwork.IsMasterClient)
            GetComponent<PhotonView>().RPC("RPC_UpdateNames", RpcTarget.Others, ((NPC)enemy1).Name, enemy2 == null ? "" : ((NPC)enemy2).Name, enemy3 == null ? "" : ((NPC)enemy3).Name, enemy4 == null ? "" : ((NPC)enemy4).Name);

        OverviewList = new List<EntityInfoFieldManager>() {
            Player1Overview.GetComponent<EntityInfoFieldManager>(), Player2Overview.GetComponent<EntityInfoFieldManager>(),
            Player3Overview.GetComponent<EntityInfoFieldManager>(), Player4Overview.GetComponent<EntityInfoFieldManager>(),
            NPC1Overview.GetComponent<EntityInfoFieldManager>(), NPC2Overview.GetComponent<EntityInfoFieldManager>(),
            NPC3Overview.GetComponent<EntityInfoFieldManager>(), NPC4Overview.GetComponent<EntityInfoFieldManager>() };
        EntityList = new List<Entity> { player1, player2, player3, player4, enemy1, enemy2, enemy3, enemy4 };

        UpdateAllOverviews();
    }

    public static Entity NPCCreation(EntityType type, int entityID) // Creates an NPC 
    {
        Entity tmp = null;
        switch (type)
        {
            case EntityType.JackOLantern:
                tmp = new JackOLantern(entityID);
                break;

            case EntityType.Skelly:
                tmp = new Skelly(entityID);
                break;

            case EntityType.WispBlue:
                tmp = new WispBlue(entityID);
                break;

            case EntityType.WispRed:
                tmp = new WispRed(entityID);
                break;

            case EntityType.BunBun:
                tmp = new BunBun(entityID);
                break;

        }

        return tmp;

    }

    public static string GetModel(EntityType character) // links the entity type to the player model
    {
        switch (character)
        {
            case EntityType.JackOLantern:
                return "Models/Anima/JackOLantern";
            case EntityType.Skelly:
                return "Models/Anima/Skelly";
            case EntityType.WispBlue:
                return "Models/Anima/WipsBlue";
            case EntityType.WispRed:
                return "Models/Anima/WipsRed";
            case EntityType.BunBun:
                return "Models/Anima/Rabbit";
        }

        return "";
    }

    public void UpdateAllOverviews() // Updates all overview fields
    {
        if (player1 != null)
            Player1Overview.GetComponent<EntityInfoFieldManager>().UpdateOverview();
        if (player2 != null)
            Player2Overview.GetComponent<EntityInfoFieldManager>().UpdateOverview();
        if (player3 != null)
            Player3Overview.GetComponent<EntityInfoFieldManager>().UpdateOverview();
        if (player4 != null)
            Player4Overview.GetComponent<EntityInfoFieldManager>().UpdateOverview();

        if (enemy1 != null)
            NPC1Overview.GetComponent<EntityInfoFieldManager>().UpdateOverview();
        if (enemy2 != null)
            NPC2Overview.GetComponent<EntityInfoFieldManager>().UpdateOverview();
        if (enemy3 != null)
            NPC3Overview.GetComponent<EntityInfoFieldManager>().UpdateOverview();
        if (enemy4 != null)
            NPC4Overview.GetComponent<EntityInfoFieldManager>().UpdateOverview();
    }

    public void ApplyTargetable(TargetStyle target, Entity entity) // Adapts Target layout depending on the wanted target style
    {
        if (player1 != null)
            Player1Overview.GetComponent<EntityInfoFieldManager>().ApplyTargetable(target, entity);
        if (player2 != null)
            Player2Overview.GetComponent<EntityInfoFieldManager>().ApplyTargetable(target, entity);
        if (player3 != null)
            Player3Overview.GetComponent<EntityInfoFieldManager>().ApplyTargetable(target, entity);
        if (player4 != null)
            Player4Overview.GetComponent<EntityInfoFieldManager>().ApplyTargetable(target, entity);

        if (enemy1 != null)
            NPC1Overview.GetComponent<EntityInfoFieldManager>().ApplyTargetable(target, entity);
        if (enemy2 != null)
            NPC2Overview.GetComponent<EntityInfoFieldManager>().ApplyTargetable(target, entity);
        if (enemy3 != null)
            NPC3Overview.GetComponent<EntityInfoFieldManager>().ApplyTargetable(target, entity);
        if (enemy4 != null)
            NPC4Overview.GetComponent<EntityInfoFieldManager>().ApplyTargetable(target, entity);
    }

    public void RemoveAllAttackRecaps() // Removes all the target recap fields
    {
        if (player1 != null)
            Player1Overview.GetComponent<EntityInfoFieldManager>().RemoveAttackRecap();
        if (player2 != null)
            Player2Overview.GetComponent<EntityInfoFieldManager>().RemoveAttackRecap();
        if (player3 != null)
            Player3Overview.GetComponent<EntityInfoFieldManager>().RemoveAttackRecap();
        if (player4 != null)
            Player4Overview.GetComponent<EntityInfoFieldManager>().RemoveAttackRecap();

        if (enemy1 != null)
            NPC1Overview.GetComponent<EntityInfoFieldManager>().RemoveAttackRecap();
        if (enemy2 != null)
            NPC2Overview.GetComponent<EntityInfoFieldManager>().RemoveAttackRecap();
        if (enemy3 != null)
            NPC3Overview.GetComponent<EntityInfoFieldManager>().RemoveAttackRecap();
        if (enemy4 != null)
            NPC4Overview.GetComponent<EntityInfoFieldManager>().RemoveAttackRecap();
    }

    public List<Entity> GetPlayers() // Returns list of active players
    {
        List<Entity> AlliesList = new List<Entity>() { };

        if (player1 != null && !player1.IsKo)
        { AlliesList.Add(player1); }
        if (player2 != null && !player2.IsKo)
        { AlliesList.Add(player2); }
        if (player3 != null && !player3.IsKo)
        { AlliesList.Add(player3); }
        if (player4 != null && !player4.IsKo)
        { AlliesList.Add(player4); }

        return AlliesList;
    }

    public List<Entity> GetNPCs() // Return list of active enemies
    {
        List<Entity> EnemiesList = new List<Entity>() { };

        if (enemy1 != null && !enemy1.IsKo)
        { EnemiesList.Add(enemy1); }
        if (enemy2 != null && !enemy2.IsKo)
        { EnemiesList.Add(enemy2); }
        if (enemy3 != null && !enemy3.IsKo)
        { EnemiesList.Add(enemy3); }
        if (enemy4 != null && !enemy4.IsKo)
        { EnemiesList.Add(enemy4); }

        return EnemiesList;
    }

    public float GetTeamHealthLevel() // Returns the procentual HP of the Team
    {
        List<Entity> AlliesList = GetPlayers();
        float CurrentHealthLevel = 0;
        float MaxHealthLevel = 0;

        foreach (Entity entity in AlliesList)
        {
            CurrentHealthLevel += entity.Hp;
            MaxHealthLevel += entity.MaxHp;
        }

        return MaxHealthLevel == 0 ? 0 : (CurrentHealthLevel / MaxHealthLevel);
    }

    public Entity FindTarget(string TargetPattern, TargetStyle TargetType, Entity caster) // Chooses A Random Valid Target For an NPC based on a Target Pattern
    {
        List<Entity> PossibleTargets = new List<Entity>() { };

        if (caster is NPC)
        {
            if (TargetType == TargetStyle.Enemies || TargetType == TargetStyle.AoEEnemies)
            { PossibleTargets = GetPlayers(); }
            if (TargetType == TargetStyle.Teammates || TargetType == TargetStyle.AoETeammates)
            { PossibleTargets = GetNPCs(); }
        }
        if (caster is Player)
        {
            if (TargetType == TargetStyle.Enemies || TargetType == TargetStyle.AoEEnemies || TargetType == TargetStyle.BaseAttack)
            { PossibleTargets = GetNPCs(); }
            if (TargetType == TargetStyle.Teammates || TargetType == TargetStyle.AoETeammates)
            { PossibleTargets = GetPlayers(); }
        }

        if (TargetType == TargetStyle.Self)
        { return caster; };

        if (PossibleTargets.Count == 0)
        { return null; }

        Entity target = PossibleTargets[0];

        switch (TargetPattern)
        {
            case "Lowest HP":
                foreach (Entity e in PossibleTargets)
                {
                    if (e.Hp < target.Hp)
                    { target = e; }
                }
                break;
            case "Highest HP":
                foreach (Entity e in PossibleTargets)
                {
                    if (e.Hp > target.Hp)
                    { target = e; }
                }
                break;
            case "Random":
                target = PossibleTargets[Random.Range(0, PossibleTargets.Count)];
                break;
        }

        return target;
    }

    public Entity FindTargetSpecial(TargetStyle targetStyle)
    {
        List<Entity> PossibleTargets = GetComponent<EntityCreation>().GetPlayers();

        System.Func<Entity, bool> condition = null;

        switch (targetStyle)
        {
            case TargetStyle.Revive:
                condition = (e => e.IsKo);
                break;
            case TargetStyle.Karim:
                condition = (e => !e.IsKo && e.Type == EntityType.Crusader);
                break;
            case TargetStyle.지영아:
                condition = (e => !e.IsKo && e.Type == EntityType.Priestess);
                break;
        }

        foreach (Entity e in PossibleTargets)
            if (condition(e))
                return e;

        return null;
    }

    public GameObject FindTargetPosition(Entity target) // Returns the corresponding player spot
    {
        if (player1 != null && player1 == target)
        { return Player1SpawnPoint; }
        if (player2 != null && player2 == target)
        { return Player2SpawnPoint; }
        if (player3 != null && player3 == target)
        { return Player3SpawnPoint; }
        if (player4 != null && player4 == target)
        { return Player4SpawnPoint; }

        if (enemy1 != null && enemy1 == target)
        { return NPC1SpawnPoint; }
        if (enemy2 != null && enemy2 == target)
        { return NPC2SpawnPoint; }
        if (enemy3 != null && enemy3 == target)
        { return NPC3SpawnPoint; }
        if (enemy4 != null && enemy4 == target)
        { return NPC4SpawnPoint; }

        return null;
    }

    public GameObject FindEnemyOverviewField(Entity target) // Returns the enemies overviewfield
    {
        if (enemy1 != null && enemy1 == target)
        { return NPC1Overview; }
        if (enemy2 != null && enemy2 == target)
        { return NPC2Overview; }
        if (enemy3 != null && enemy3 == target)
        { return NPC3Overview; }
        if (enemy4 != null && enemy4 == target)
        { return NPC4Overview; }

        return null;
    }

    public void DeathOrRevive() // Activates or Deactivates Player Models, depending on whether they are KO
    {
        if (player1 != null)
        {
            if (player1.IsKo)
            { player1Model.SetActive(false); }
            else
            { player1Model.SetActive(true); }
        }

        if (player2 != null)
        {
            if (player2.IsKo)
            { player2Model.SetActive(false); }
            else
            { player2Model.SetActive(true); }
        }

        if (player3 != null)
        {
            if (player3.IsKo)
            { player3Model.SetActive(false); }
            else
            { player3Model.SetActive(true); }
        }

        if (player4 != null)
        {
            if (player4.IsKo)
            { player4Model.SetActive(false); }
            else
            { player4Model.SetActive(true); }
        }


        if (enemy1 != null)
        {
            if (enemy1.IsKo)
            { NPC1Model.SetActive(false); }
            else
            { NPC1Model.SetActive(true); }
        }

        if (enemy2 != null)
        {
            if (enemy2.IsKo)
            { NPC2Model.SetActive(false); }
            else
            { NPC2Model.SetActive(true); }
        }

        if (enemy3 != null)
        {
            if (enemy3.IsKo)
            { NPC3Model.SetActive(false); }
            else
            { NPC3Model.SetActive(true); }
        }

        if (enemy4 != null)
        {
            if (enemy4.IsKo)
            { NPC4Model.SetActive(false); }
            else
            { NPC4Model.SetActive(true); }
        }
    }

    public void FlashUpEffect(Entity target) // Makes the Entity's model flash up red when receiving damage (Finds the model)
    {
        GameObject ModelToAffect = null;

        if (player1 == target)
        { ModelToAffect = player1Model; }
        if (player2 == target)
        { ModelToAffect = player2Model; }
        if (player3 == target)
        { ModelToAffect = player3Model; }
        if (player4 == target)
        { ModelToAffect = player4Model; }

        if (enemy1 == target)
        { ModelToAffect = NPC1Model; }
        if (enemy2 == target)
        { ModelToAffect = NPC2Model; }
        if (enemy3 == target)
        { ModelToAffect = NPC3Model; }
        if (enemy4 == target)
        { ModelToAffect = NPC4Model; }

        StartCoroutine(FlashUpColour(ModelToAffect));
    }

    private IEnumerator FlashUpColour(GameObject ModelToAffect) // Delayed component of the function above (Applies colour change)
    {
        Color red = new Color(0.90f, 0.00f, 0.30f, 1f);

        MeshRenderer[] BodyParts = ModelToAffect.GetComponentsInChildren<MeshRenderer>();
        List<Color> BodyPartsInitialColour = new List<Color>() { };

        SkinnedMeshRenderer[] BodyParts2 = ModelToAffect.GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Color> BodyPartsInitialColour2 = new List<Color>() { };

        foreach (MeshRenderer mr in BodyParts)
        {
            BodyPartsInitialColour.Add(mr.material.color);
            mr.material.color = red;
        }

        foreach (SkinnedMeshRenderer smr in BodyParts2)
        {
            BodyPartsInitialColour2.Add(smr.materials[0].color);
            BodyPartsInitialColour2.Add(smr.materials[1].color);
            smr.materials[0].color = red;
            smr.materials[1].color = red;
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < BodyParts.Length; i++)
        {
            BodyParts[i].material.color = BodyPartsInitialColour[i];
        }

        for (int i = 0; i < BodyParts2.Length; i++)
        {
            BodyParts2[i].materials[0].color = BodyPartsInitialColour2[2*i];
            BodyParts2[i].materials[1].color = BodyPartsInitialColour2[2*i+1];
        }
    }

    public void CreatePopUpIcon(Entity entity, int value, string Type) // Makes the Pop Up Icon appear (Damage / Heal / Mana / Dodge)
    {
        Color ColourToUse = new Color(0f, 0f, 0f, 0f);

        switch (Type)
        {
            case "Damage":
                ColourToUse = new Color(0.90f, 0.00f, 0.30f, 1f); //red
                break;
            case "Mana":
                ColourToUse = new Color(0.20f, 0.87f, 1.00f, 1f); // blue
                break;
            case "Heal":
                ColourToUse = new Color(0.20f, 1.00f, 0.33f, 1f); // green
                break;
            case "Dodged":
                ColourToUse = new Color(0.70f, 0.75f, 0.71f, 1f);
                break;
        }

        Object tmp = Instantiate(Resources.Load("Pop Up Icon"), FindTargetPosition(entity).transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);

        ((GameObject)tmp).GetComponent<PopUpIconManager>().SetTextAndColor(value, ColourToUse);
    }

    /////////////////////////////////////////////////////////////// RPC

    [PunRPC]
    private void RPC_UpdateNames(string name1, string name2, string name3, string name4)
    {
        ((NPC)enemy1).Name = name1;
        NPC1Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((NPC)enemy1).Name, enemy1);

        if (name2 != "")
        {
            ((NPC)enemy2).Name = name2;
            NPC2Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((NPC)enemy2).Name, enemy2);
        }

        if (name3 != "")
        {
            ((NPC)enemy3).Name = name3;
            NPC3Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((NPC)enemy3).Name, enemy3);
        }

        if (name4 != "")
        {
            ((NPC)enemy4).Name = name4;
            NPC4Overview.GetComponent<EntityInfoFieldManager>().SetNameAndEntity(((NPC)enemy4).Name, enemy4);
        }
    }

    [PunRPC]
    public void RPC_LoseHealth(int attackerID, int targetID, int damage, float DodgeChance)
    // Loses Health BUT for RPC Calls (only called once in the usage of Attack (since that is the only case where a dodge can happen -> thus being the sole factor with desynchronization))
    {
        List<Entity> EntityList = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().EntityList;

        Entity CurrentAttacker = EntityList[attackerID];
        Entity CurrentTarget = EntityList[targetID];

        if (DodgeChance <= CurrentTarget.DodgeRate)
        {
            GetComponent<EntityCreation>().CreatePopUpIcon(CurrentTarget, 0, "Dodged");
            return;
        }

        int mitigatedDamage = (int)(damage - damage * CurrentTarget.ArmorBuffDebuff);
        CurrentTarget.Hp -= mitigatedDamage;

        CurrentAttacker.LoseHealth((int)(damage * CurrentTarget.RipostePercentage), null, false, false);

        if (damage != 0)
        { GetComponent<EntityCreation>().CreatePopUpIcon(CurrentTarget, mitigatedDamage, "Damage"); }
        if (CurrentTarget.Hp > 0)
        {
            GetComponent<EntityCreation>().FlashUpEffect(CurrentTarget);
            return;
        }

        CurrentTarget.Hp = 0;
        CurrentTarget.Mana = 0;
        CurrentTarget.IsKo = true;
        GetComponent<EntityCreation>().DeathOrRevive();
        return;
    }

    [PunRPC]
    public void RPC_Stun(int targetID)
    {
        Entity CurrentTarget = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().EntityList[targetID];
        CurrentTarget.IsStunned = true;
    }
}
