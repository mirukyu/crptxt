using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* General Structure:
 * 1) Setup: Username, Amount Attacks
 * 2) Constructor
 * 3) Methods: Get Attack Name General, Get Attack Description General, Get Attack Details General, Get Attack Target General, Use Spell General
 */

public class Player : Entity // Do Ashley as secret acharacter
{
    #region Setup
    private string username;
    private int amountAttacks;

    public string Username
    {
        get { return username; }
        set { username = value; }
    }
    public int AmountAttacks
    {
        get { return amountAttacks; }
        set { amountAttacks = value; }
    }
    #endregion

    #region Constructor
    public Player(string username, int entityID, EntityType type, string typeName,int hp, int mana, int baseStrength, float dodgeRate, int amountAttacks)
            : base(entityID, type, typeName, hp, mana, baseStrength, dodgeRate)
    {
        this.username = username;
        this.amountAttacks = amountAttacks;
    }
    #endregion

    #region Methods
    public string GetAttackNameGeneral (int index, Entity entity)
    {
        switch (entity.Type)
        {
            case EntityType.Aramusha:
                return ((Aramusha)entity).GetAttackName(index);
            case EntityType.Priestess:
                return ((Priestress)entity).GetAttackName(index);
            case EntityType.Mage:
                return ((Mage)entity).GetAttackName(index);
            case EntityType.Crusader:
                return ((Crusader)entity).GetAttackName(index);
            default:
                return "";
        }
    }

    public string GetAttackDescriptionGeneral (int index, Entity entity)
    {
        switch (entity.Type)
        {
            case EntityType.Aramusha:
                return ((Aramusha)entity).GetAttackDescription(index);
            case EntityType.Priestess:
                return ((Priestress)entity).GetAttackDescription(index);
            case EntityType.Mage:
                return ((Mage)entity).GetAttackDescription(index);
            case EntityType.Crusader:
                return ((Crusader)entity).GetAttackDescription(index);
            default:
                return "";
        }
    }

    public string GetAttackDetailsGeneral(int index, Entity entity)
    {
        switch (entity.Type)
        {
            case EntityType.Aramusha:
                return ((Aramusha)entity).GetAttackDetail(index);
            case EntityType.Priestess:
                return ((Priestress)entity).GetAttackDetail(index);
            case EntityType.Mage:
                return ((Mage)entity).GetAttackDetail(index);
            case EntityType.Crusader:
                return ((Crusader)entity).GetAttackDetail(index);
            default:
                return "";
        }
    }

    public int GetManaRequirementGeneral (int index, Entity entity)
    {
        if (index == 7)
            return 0;

        switch (entity.Type)
        {
            case EntityType.Aramusha:
                return ((Aramusha)entity).GetManaRequirement(index);
            case EntityType.Priestess:
                return ((Priestress)entity).GetManaRequirement(index);
            case EntityType.Mage:
                return ((Mage)entity).GetManaRequirement(index);
            case EntityType.Crusader:
                return ((Crusader)entity).GetManaRequirement(index);
            default:
                return 0;
        }
    }

    public TargetStyle GetAttackTargetGeneral (int index, Entity entity)
    {
        if (index == 7)
            return TargetStyle.Enemies;

        switch (entity.Type)
        {
            case EntityType.Aramusha:
                return ((Aramusha)entity).GetAttackTarget(index);
            case EntityType.Priestess:
                return ((Priestress)entity).GetAttackTarget(index);
            case EntityType.Mage:
                return ((Mage)entity).GetAttackTarget(index);
            case EntityType.Crusader:
                return ((Crusader)entity).GetAttackTarget(index);
            default:
                return TargetStyle.Default;
        }
    }

    public void UseSpellGeneral (int index, Entity attacker, Entity target)
    {
        switch (attacker.Type)
        {
            case EntityType.Aramusha:
                ((Aramusha)attacker).UseSpell(index, target);
                break;
            case EntityType.Priestess:
                ((Priestress)attacker).UseSpell(index, target);
                break;
            case EntityType.Mage:
                ((Mage)attacker).UseSpell(index, target);
                break;
            case EntityType.Crusader:
                ((Crusader)attacker).UseSpell(index, target);
                break;
        }
    }
    #endregion
}
