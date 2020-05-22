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

    // Here we will save all the attacks, wanted to be executed by players, the structure is the following: [attackerID, targetID, attackIndex]
    private List<List<int>> AttackList = new List<List<int>>();

    public void Start()
    {
        myID = CharacterSetUp.Game.myID;

        EntityCreation tmp = GetComponent<EntityCreation>();

        EntityList = tmp.EntityList;
        OverviewFieldList = tmp.OverviewList;  

        attacker = EntityList[myID];

        GetComponent<TextManager>().FillInAttackFields(attacker);
        GetComponent<TextManager>().BackToActionSelect();

        PV = GetComponent<PhotonView>();

        StartCoroutine(PlayAudio());
    }

    public IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(0.1f);
        if (GetComponent<EntityCreation>().BossBattle)
            GetComponent<AudioManager>().PlayTheme(ThemeStyle.Boss);
        else
            GetComponent<AudioManager>().PlayTheme(ThemeStyle.Battle);
    }

    ///////////////////////////////

    public void TargetButton(GameObject overviewField) // Takes input of the pressed Entity and takes appropriate measure
    {
        target = overviewField.GetComponent<EntityInfoFieldManager>().myEntity;

        GetComponent<TextManager>().SetBarrier();


        if (spellIndex == 7)
            GetComponent<TextManager>().OutputTextMinor(attacker.TypeName + " => " + "Base Attack" + " => " + target.TypeName);
        else
            GetComponent<TextManager>().OutputTextMinor(attacker.TypeName + " => " + ((Player)attacker).GetAttackNameGeneral(spellIndex, attacker) + " => " + target.TypeName);

        PV.RPC("RPC_SetAttackRecap", RpcTarget.All, attacker.EntityID, target.EntityID, spellIndex);
        GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Default, null);

        PV.RPC("RPC_AddAttack2AttackList", RpcTarget.MasterClient, attacker.EntityID, target.EntityID, spellIndex);
    }

    public void ChooseAttack(int i) // Takes input of the Attack Button
    {
        if (i == 7)
        {
            spellIndex = i;
            GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Enemies, null);
            GetComponent<TextManager>().OutputTextMinor(attacker.TypeName + " => Base Attack =>");
        }
        else
        {
            if (attacker.IsExhausted)
            {
                GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Default, null);
                GetComponent<TextManager>().OutputTextMinor(" !!! You are exhausted! You can only do Basic Attacks !!!");
                return;
            }

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

    private IEnumerator FullAttackExecution()
    {
        yield return new WaitForSeconds(1f);

        PV.RPC("RPC_RemoveAttackRecap", RpcTarget.All);

        FillAttackListWithEnemies();

        List<int> Used = new List<int>();

        while (Used.Count != AttackList.Count)
        {
            int index = Random.Range(0, AttackList.Count);

            while (Used.Contains(index))
                index = Random.Range(0, AttackList.Count);

            Used.Add(index);

            PV.RPC("RPC_ExecuteAttack", RpcTarget.All, AttackList[index][0], AttackList[index][1], AttackList[index][2], true, true);

            yield return new WaitForSeconds(GameObject.Find("SFX Manager").GetComponent<SFXManager>().GetClipLength() + 0.5f);
        }

        yield return new WaitForSeconds(1f);
        AttackList = new List<List<int>>();
        PV.RPC("RPC_Reset", RpcTarget.All);
        PV.RPC("RPC_RemoveAttackRecap", RpcTarget.All);

        if (GetComponent<EntityCreation>().GetPlayers().Count == 0) // Win & Lose Criteria
            GetComponent<GameManager>().Return2Traversal("Lose");
        else if (GetComponent<EntityCreation>().GetNPCs().Count == 0)
            GetComponent<GameManager>().Return2Traversal("Win");

        PV.RPC("RPC_VerfiyState", RpcTarget.All);
    }

    private void FillAttackListWithEnemies()
    {
        List<Entity> PossibleEnemies = GetComponent<EntityCreation>().GetNPCs();

        foreach (Entity caster in PossibleEnemies)
        {
            int attackIndex = Random.Range(0, ((NPC)caster).AmountAttacks);
            TargetStyle attackTarget = ((NPC)caster).GetAttackTargetGeneral(attackIndex, caster);
            Entity target = GetComponent<EntityCreation>().FindTarget("Random", attackTarget, caster);

            AttackList.Add(new List<int> { caster.EntityID, target.EntityID, attackIndex });
        }
    }

    public void ResetAll()
    {
        target = null;
        spellIndex = -1;
    }

    public void FleeAttempt()
    {
        if (Random.Range(0f, 1f) >= 0.9f * GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetTeamHealthLevel())
        {
            GetComponent<GameManager>().SuccessfullFleeing();
            GetComponent<TextManager>().OutputTextMinor("You could successfully flee!");
        }
        else
        {
            GetComponent<TextManager>().OutputTextMinor("Failure! Keep fighting you coward!");
            PV.RPC("FailedFlee", RpcTarget.All, attacker.EntityID);
        }
    }

    ///////////////////////////////////////////////////// RPC Calls

    [PunRPC]
    private void RPC_ExecuteAttack(int attackerID, int targetID, int attackIndex, bool ApplyConfuse, bool ApplyBlind)
    {
        Entity CurrentAttacker = EntityList[attackerID];
        Entity CurrentTarget = EntityList[targetID];

        if (CurrentAttacker.IsBleeding) // Bleed
            CurrentAttacker.Bleed();

        // Chacking all Ailments
        if (CurrentAttacker.IsKo) // Died in Combat
        {
            PV.RPC("RPC_SetAttackRecapSpecial", RpcTarget.All, attackerID, "Died");
            return;
        }

        if (CurrentAttacker.IsStunned) // Stun
        {
            PV.RPC("RPC_SetAttackRecapSpecial", RpcTarget.All, attackerID, "Stunned");
            CurrentAttacker.IsStunned = false;
            return;
        }

        if (CurrentAttacker.IsExhausted) // Exhaustion
            CurrentAttacker.IsExhausted = false;

        if (CurrentAttacker.IsConfused && ApplyConfuse) // Confusion
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (attackerID <= 3)
                {
                    attackIndex = Random.Range(0, ((Player)CurrentAttacker).AmountAttacks);
                    if (((Player)CurrentAttacker).GetManaRequirementGeneral(attackIndex, attacker) > CurrentAttacker.Mana || CurrentAttacker.IsExhausted)
                        attackIndex = 7;
                }
                else
                    attackIndex = Random.Range(0, ((NPC)CurrentAttacker).AmountAttacks);
                PV.RPC("RPC_ExecuteAttack", RpcTarget.All, attackerID, targetID, attackIndex, false, ApplyBlind);
            }
            return;
        }

        if (CurrentAttacker.IsBlinded && ApplyBlind) // Blind
        {
            if (PhotonNetwork.IsMasterClient)
            {
                TargetStyle attackTarget = TargetStyle.Default;
                if (attackerID <= 3)
                    attackTarget = ((Player)CurrentAttacker).GetAttackTargetGeneral(attackIndex, CurrentAttacker);
                else
                    attackTarget = ((NPC)CurrentAttacker).GetAttackTargetGeneral(attackIndex, CurrentAttacker);

                if (attackTarget == TargetStyle.Revive || attackTarget == TargetStyle.지영아 || attackTarget == TargetStyle.Karim)
                {
                    Entity tmpTarget = GetComponent<EntityCreation>().FindTargetSpecial(attackTarget);

                    if (attackTarget == TargetStyle.지영아 && tmpTarget == null)
                        tmpTarget = GetComponent<EntityCreation>().FindTarget("Random", TargetStyle.Enemies, CurrentAttacker);

                    if (tmpTarget == null)
                    {
                        PV.RPC("RPC_ExecuteAttack", RpcTarget.All, attackerID, CurrentTarget.EntityID, attackIndex, ApplyConfuse, true);
                        return;
                    }

                    CurrentTarget = tmpTarget;
                }
                else
                    CurrentTarget = GetComponent<EntityCreation>().FindTarget("Random", attackTarget, CurrentAttacker);

                PV.RPC("RPC_ExecuteAttack", RpcTarget.All, attackerID, CurrentTarget.EntityID, attackIndex, ApplyConfuse, false);
            }
            return;
        }

        // Actual Execution of the Attack
        if (attackIndex == 7)
        {
            ((Player)CurrentAttacker).BaseAttack(CurrentTarget);
            GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("Punch", "Persona");
        }
        else
        {
            if (attackerID <= 3)
                ((Player)CurrentAttacker).UseSpellGeneral(attackIndex, CurrentAttacker, CurrentTarget);
            else
                ((NPC)CurrentAttacker).UseSpellGeneral(attackIndex, CurrentAttacker, CurrentTarget);
        }

        PV.RPC("RPC_OverviewUpdate", RpcTarget.All);
        PV.RPC("RPC_SetAttackRecap", RpcTarget.All, attackerID, targetID, attackIndex);
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
        {
            if (attackerID <= 3)
                CurrentOverviewField.SetAttackRecap(CurrentAttacker.TypeName + "\n => \n" + ((Player)CurrentAttacker).GetAttackNameGeneral(attackIndex, CurrentAttacker) + "\n => \n" + CurrentTarget.TypeName);
            else
                CurrentOverviewField.SetAttackRecap(CurrentAttacker.TypeName + "\n => \n" + ((NPC)CurrentAttacker).GetAttackNameGeneral(attackIndex, CurrentAttacker) + "\n => \n" + CurrentTarget.TypeName);
        }     
    }

    [PunRPC]
    private void RPC_SetAttackRecapSpecial(int attackerID, string text)
    {
        Entity CurrentAttacker = EntityList[attackerID];
        EntityInfoFieldManager CurrentOverviewField = OverviewFieldList[attackerID];

        CurrentOverviewField.SetAttackRecap(text + " !");
    }

    [PunRPC]
    private void RPC_RemoveAttackRecap()
    {
        foreach (EntityInfoFieldManager CurrentOverviewField in OverviewFieldList)
            CurrentOverviewField.RemoveAttackRecap();
    }

    [PunRPC]
    private void RPC_AddAttack2AttackList(int attackerID, int targetID, int attackIndex)
    {
        AttackList.Add(new List<int> { attackerID, targetID, attackIndex});

        if (GetComponent<EntityCreation>().GetTeamHealthLevel() != 0 && AttackList.Count == RoomController.room.playersInRoom)
            StartCoroutine(FullAttackExecution());
    }

    [PunRPC]
    private void RPC_OverviewUpdate()
    {
        GetComponent<EntityCreation>().UpdateAllOverviews();
        GetComponent<EntityCreation>().DeathOrRevive(); ///////////////////
    }

    [PunRPC]
    private void RPC_Reset()
    {
        ResetAll();

        GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Default, null);

        GetComponent<EntityCreation>().UpdateAllOverviews();
        GetComponent<TextManager>().BackToActionSelect();
    }

    [PunRPC]
    private void RPC_VerfiyState()
    {
        // This function simply checks whether the players have only one action to do (aka are stunned or dead)

        if (attacker.IsKo) // Is Dead
        {
            GetComponent<TextManager>().SetBarrier();
            GetComponent<TextManager>().OutputTextMinor("You are Dead!");
            PV.RPC("RPC_SetAttackRecapSpecial", RpcTarget.All, attacker.EntityID, "Died");
            PV.RPC("RPC_AddAttack2AttackList", RpcTarget.MasterClient, attacker.EntityID, 0, 0);
        }

        else if (attacker.IsStunned) // Stun
        {
            GetComponent<TextManager>().SetBarrier();
            GetComponent<TextManager>().OutputTextMinor("You are Stunned!");
            PV.RPC("RPC_SetAttackRecapSpecial", RpcTarget.All, attacker.EntityID, "Stunned");
            PV.RPC("RPC_AddAttack2AttackList", RpcTarget.MasterClient, attacker.EntityID, 0, 0);
        }
    }

    [PunRPC]
    void FailedFlee(int loserID)
    {
        EntityList[loserID].IsStunned = true;

        if (attacker.EntityID == loserID)
        {
            GetComponent<TextManager>().SetBarrier();
            PV.RPC("RPC_SetAttackRecapSpecial", RpcTarget.All, attacker.EntityID, "Flee Attempt");
            PV.RPC("RPC_AddAttack2AttackList", RpcTarget.MasterClient, attacker.EntityID, 0, 0);
        }
    }
}
