using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/* General Structure:
 * 1) Setup: Name, NPC Name List
 * 2) Constructor
 * 3) Methods: Get Attack Name General, Get Attack Target General, Use Spell General
 */

public class NPC : Entity
{
    #region Setup
    private string name;
    private string[] NPCNameList = new string[] { "Wilhelm", "Paul", "Friedrich",
        "Elongated Musket", "Mike Okzlong", "Jessie", "Ben Doover", "Frank", "Maria",
        "Reinhardt", "Sophia", "Emma", "Wolfie", "Ditto"};

    private int amountAttacks;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public int AmountAttacks
    {
        get { return amountAttacks; }
        set { amountAttacks = value; }
    }
    #endregion

    #region Constructor
    public NPC(int entityID, EntityType type, string typeName, int hp, int baseStrength, float dodgeRate, int amountAttacks)
            : base(entityID, type, typeName, hp, 100, baseStrength, dodgeRate)
    {
        name = NPCNameList[Random.Range(0, (NPCNameList.Length) - 1)];
        
        this.amountAttacks = amountAttacks;

        
    }
    #endregion

    #region Methods
    public string GetAttackNameGeneral(int index, Entity entity)
    {
        switch (entity.Type)
        {
            case EntityType.JackOLantern:
                return ((JackOLantern)entity).GetAttackName(index);
            default:
                return "";
        }
    }

    public TargetStyle GetAttackTargetGeneral(int index, Entity entity)
    {
        switch (entity.Type)
        {
            case EntityType.JackOLantern:
                return ((JackOLantern)entity).GetAttackTarget(index);
            default:
                return TargetStyle.Default;
        }
    }

    public void UseSpellGeneral(int index, Entity attacker, Entity target)
    {
        switch (attacker.Type)
        {
            case EntityType.JackOLantern:
                ((JackOLantern)attacker).UseSpell(index, target);
                break;
        }
    }
    #endregion
}
