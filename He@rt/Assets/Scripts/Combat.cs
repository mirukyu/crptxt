using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour {                 // SPELLINDEX 7 IS USED FOR BASE ATTACKS

    private Entity attacker;
    private Entity target;
    private int spellIndex = -1;
    private int myID;

    private GameObject MyOverviewField;

    private List<Entity> EntityList;
    private List<EntityInfoFieldManager> OverviewFieldList;

    private PhotonView PV;

    public void Start()
    {
        myID = CharacterSetUp.Game.myID;

        EntityCreation tmp = GetComponent<EntityCreation>();

        EntityList = tmp.EntityList;
        OverviewFieldList = tmp.OverviewList;  

        attacker = EntityList[myID];

        GetComponent<TextManager>().FillInAttackFields(attacker);
        GetComponent<TextManager>().OutputTextMinor(attacker.TypeName + " =>");

        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update ()
    {
        //if (attacker != null && target != null && spellIndex != -1) // Executing an Attack (all other side actions needed)
        if (target != null && spellIndex != -1)
        {
            PV.RPC("RPC_ExecuteAttack", RpcTarget.All, attacker.EntityID, target.EntityID, spellIndex);
            /*
            GetComponent<EntityCreation>().RemoveAllAttackRecaps();

            if (spellIndex == 7)
            {
                attacker.Attack(target, attacker.BaseStrength);
                GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("Punch", "Persona");
                CurrentOverviewField.GetComponent<EntityInfoFieldManager>().SetAttackRecap(attacker.TypeName + "\n => \n" + "Base Attack" + "\n => \n" + target.TypeName);
            }
            else
            {
                //ExecuteAttack();
                PV.RPC("RPC_ExecuteAttack", RpcTarget.All, attacker.EntityID, target.EntityID, spellIndex);
                CurrentOverviewField.GetComponent<EntityInfoFieldManager>().SetAttackRecap(attacker.TypeName + "\n => \n" + ((Player)attacker).GetAttackNameGeneral(spellIndex, attacker) + "\n => \n" + target.TypeName);
                
            }
            */
            GetComponent<EntityCreation>().UpdateAllOverviews();

            StartCoroutine(EnemyCounterAttack());

            
            Reset();
        }
	}

    public void TargetButton(GameObject overviewField) // Takes input of the pressed Entity and takes appropriate measure
    {
        target = overviewField.GetComponent<EntityInfoFieldManager>().myEntity;

        GetComponent<TextManager>().SetBarrier();
        PV.RPC("RPC_SetAttackRecap", RpcTarget.All, attacker.EntityID, target.EntityID, spellIndex);
        // Add attack to list ////////////////////////////////////////////////////////////////////////////////////
    }

    public void ChooseAttack(int i) // Takes input of the Attack Button
    {
        if (i == 7)
        {
            spellIndex = i;
            //GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.BaseAttack, null);
            GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Enemies, null);
            GetComponent<TextManager>().OutputTextMinor(attacker.TypeName + " => Base Attack =>");
        }
        else
        {
            if (((Player)attacker).GetManaRequirementGeneral(i, attacker) <= attacker.Mana)
            {
                spellIndex = i;

                GetComponent<EntityCreation>().ApplyTargetable(((Player)attacker).GetAttackTargetGeneral(i, attacker), attacker);
                GetComponent<TextManager>().OutputTextMinor(attacker.TypeName + " => " + ((Player)attacker).GetAttackNameGeneral(spellIndex, attacker) + " =>");
            }
            else
            {
                spellIndex = -1;

                GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Default, null);
                GetComponent<TextManager>().OutputTextMinor(" !!! Mana is insufficient !!!");
            }
        }
    }

    public void ExecuteAttack() // Actual Execution of tha Attack
    {
        ((Player)attacker).UseSpellGeneral(spellIndex, attacker, target);
    }

    public void Reset()
    {
        GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Default, null);

        //attacker = null;
        target = null;
        spellIndex = -1;
    }

    public IEnumerator EnemyCounterAttack() // Enemy Attacking After Player's Turn
    {
        yield return new WaitForSeconds(GameObject.Find("SFX Manager").GetComponent<SFXManager>().GetClipLength() + 0.5f);

        List<Entity> PossibleEnemies = new List<Entity>() { };
        PossibleEnemies = GetComponent<EntityCreation>().GetNPCs();
        Entity caster = PossibleEnemies[Random.Range(0, PossibleEnemies.Count)]; // We get one random enemy

        int attackIndex = Random.Range(0, ((NPC)caster).AmountAttacks); // Getting all necessary information
        string attackName = ((NPC)caster).GetAttackNameGeneral(attackIndex, caster);
        TargetStyle attackTarget = ((NPC)caster).GetAttackTargetGeneral(attackIndex, caster);

        Entity target = GetComponent<EntityCreation>().FindTargetForNPC("Random", attackTarget, caster); //

        ((NPC)caster).UseSpellGeneral(attackIndex, caster, target);

        GetComponent<EntityCreation>().FindEnemyOverviewField(caster).GetComponent<EntityInfoFieldManager>().SetAttackRecap(caster.TypeName + "\n => \n" + attackName + "\n => \n" + target.TypeName);

        //////

        GetComponent<EntityCreation>().UpdateAllOverviews(); // THIS IS NECESSARY FOR RESET / WILL BE CAST AFTER CONCLUSION OF BOTH SIDE'S ATTACKS
        GetComponent<TextManager>().BackToActionSelect();
    }

    ///////////// RPC Calls

    [PunRPC]
    private void RPC_ExecuteAttack(int attackerID, int targetID, int attackIndex)
    {
        Entity CurrentAttacker = EntityList[attackerID];
        Entity CurrentTarget = EntityList[targetID];
        if (spellIndex == 7)
        {
            ((Player)CurrentAttacker).BaseAttack(CurrentTarget);
            GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("Punch", "Persona");
        }
        else
            ((Player)CurrentAttacker).UseSpellGeneral(attackIndex, CurrentAttacker, CurrentTarget);
    }

    [PunRPC]
    private void RPC_SetAttackRecap(int attackerID, int targetID, int attackIndex)
    {
        Entity CurrentAttacker = EntityList[attackerID];
        Entity CurrentTarget = EntityList[targetID];
        EntityInfoFieldManager CurrentOverviewField = OverviewFieldList[attackerID];

        if (attackIndex == 7)
            CurrentOverviewField.SetAttackRecap(CurrentAttacker.TypeName + "\n => \n" + "Base Attack" + "\n => \n" + CurrentTarget.TypeName);
        else
            CurrentOverviewField.SetAttackRecap(CurrentAttacker.TypeName + "\n => \n" + ((Player)CurrentAttacker).GetAttackNameGeneral(attackIndex, CurrentAttacker) + "\n => \n" + CurrentTarget.TypeName);
    }
}
