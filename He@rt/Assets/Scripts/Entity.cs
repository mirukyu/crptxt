using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* General Structure:
 * 1) Setup: ALL FUCKING VARIABLES, THERE ARE SO MANY KARIM (with getter and setter)
 * 2) Constructor
 * 3) Methods: Base Attack, Spend Mana, Lose Health, Heal, Regenerate Mana, Attack, Receive Damage Buff Debuff, Receive Armor Buff Debuff, Ailment Clearance, Revive, Apply AoE, HealAndManaRegenFromMinigame
 */

public enum EntityType
{
    Aramusha, Priestess, Mage, Crusader, JackOLantern, Skelly, WispBlue, WispRed, BunBun, DEFAULT
}

public class Entity
{
    #region Setup
    private int entityID;

    private EntityType type;
    private string typeName;
    private int hp;
    private int maxHp;
    private int mana;
    private int maxMana;
    private int baseStrength;
    private bool isKo;

    private float dodgeRate;
    private float damageBuffDebuff;
    private float armorBuffDebuff;
    private float ripostePercentage;
    private float crippledPercentage;

    private bool isBleeding;
    private bool isStunned;
    private bool isConfused;
    private bool isBlinded;
    private bool isExhausted;
    private bool isRiposte;
    private bool isCrippled;

    public int EntityID
    { get { return entityID; } }

    public EntityType Type
    { get { return type; } }
    public string TypeName
    { get { return typeName; } }
    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }
    public int MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }
    public int Mana
    {
        get { return mana; }
        set { mana = value; }
    }
    public int MaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }
    public int BaseStrength
    {
        get { return baseStrength; }
        set { baseStrength = value; }
    }
    public bool IsKo
    {
        get { return isKo; }
        set { isKo = value; }
    }


    public float DodgeRate
    {
        get { return dodgeRate; }
        set { dodgeRate = value; }
    }
    public float DamageBuffDebuff
    {
        get { return damageBuffDebuff; }
        set { damageBuffDebuff = value; }
    }
    public float ArmorBuffDebuff
    {
        get { return armorBuffDebuff; }
        set { armorBuffDebuff = value; }
    }
    public float RipostePercentage
    {
        get { return ripostePercentage; }
        set { ripostePercentage = value; }
    }
    public float CrippledPercentage
    {
        get { return crippledPercentage; }
        set { crippledPercentage = value; }
    }


    public bool IsBleeding
    {
        get { return isBleeding; }
        set { isBleeding = value; }
    }
    public bool IsStunned
    {
        get { return isStunned; }
        set { isStunned = value; }
    }
    public bool IsConfused
    {
        get { return isConfused; }
        set { isConfused = value; }
    }
    public bool IsBlinded
    {
        get { return isBlinded; }
        set { isBlinded = value; }
    }
    public bool IsExhausted
    {
        get { return isExhausted; }
        set { isExhausted = value; }
    }
    public bool IsRiposte
    {
        get { return isRiposte; }
        set { isRiposte = value; }
    }
    public bool IsCrippled
    {
        get { return isCrippled; }
        set { isCrippled = value; }
    }
    #endregion

    #region Constructor
    public Entity(int entityID, EntityType type, string typeName, int hp, int mana, int baseStrength, float dodgeRate)
    {
        this.entityID = entityID;

        this.type = type;
        this.typeName = typeName;
        this.hp = hp;
        this.mana = mana;
        this.baseStrength = baseStrength;
        this.dodgeRate = dodgeRate;

        maxHp = hp;
        maxMana = mana;

        damageBuffDebuff = 0f;
        armorBuffDebuff = 0f;
        ripostePercentage = 0f;
        crippledPercentage = 0f;

        isKo = false;
        isBleeding = false;
        isStunned = false;
        isConfused = false;
        isBlinded = false;
        isExhausted = false;
        isRiposte = false;
        isCrippled = false;
    }

    #endregion

    #region Methods
    public void BaseAttack(Entity target) // Spends Mana
    {
        Attack(target, baseStrength);
    }

    public bool SpendMana(int spentMana) // Spends Mana
    {
        if (mana - spentMana < 0)
        { return false; }

        mana -= spentMana;
        return true;
    }

    public string LoseHealth(int damage, Entity attacker, bool reflect, bool canDodge) // Loses Health (Taking Dodge and Reflect into Concideration)
    {
        if (canDodge && Random.Range(0.00f, 1f) <= dodgeRate)
        { return "Dodged"; }

        int mitigatedDamage = (int)(damage - damage * armorBuffDebuff);
        hp -= mitigatedDamage;
        if (reflect)
        { attacker.LoseHealth((int)(damage * ripostePercentage), this, false, false); }
        if (damage != 0)
        { GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().CreatePopUpIcon(this, mitigatedDamage, "Damage"); }
        if (hp > 0)
        { return "Survived"; }

        hp = 0;
        mana = 0;
        IsKo = true;
        return "Died";
    }

    public void Heal(int life) // Heals Health
    {
        if (hp != 0)
        { hp += life; }

        if (hp > maxHp)
        { hp = maxHp; }

        GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().CreatePopUpIcon(this, life, "Heal");
    }

    public void RegenerateMana(int newMana) // Regenerates Mana
    {
        mana += newMana;

        if (mana > maxMana)
        { mana = maxMana; }

        GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().CreatePopUpIcon(this, newMana, "Mana");
    }

    public void Bleed()
    {
        LoseHealth(5, null, false, false);
    }

    public void Attack (Entity entity, int damage) // Attacks a certain entity (with crippled) (Either Spell or Base Attack)
    {
        LoseHealth((int) (damage * crippledPercentage), this, false, false);

        if (PhotonNetwork.IsMasterClient)
        {
            float DodgeChance = Random.Range(0.00f, 1f);
            GameObject.Find("Game Manager Battle").GetComponent<PhotonView>().RPC("RPC_LoseHealth", RpcTarget.All, entityID, entity.EntityID, (int)(damage + damage * damageBuffDebuff), DodgeChance);
        }
    }

    public void ReceiveDamageBuffDebuff (float damageBuffDebuffValue) // Managing Damage Buffs and Debuffs
    {
        damageBuffDebuff += damageBuffDebuffValue;
        if (damageBuffDebuff > 1f)
        { damageBuffDebuff = 1f; }
        if (damageBuffDebuff < -1f)
        { damageBuffDebuff = -1f; }
    }

    public void ReceiveArmorBuffDebuff(float armorBuffDebuffValue) // Managing Armor Buffs and Debuffs
    {
        armorBuffDebuff += armorBuffDebuffValue;
        if (armorBuffDebuff > 1f)
        { armorBuffDebuff = 1f; }
        if (armorBuffDebuff < -1f)
        { armorBuffDebuff = -1f; }
    }

    public void AilmentClearance() // Clears all ailments statusses excpet Riposte
    {
        IsBleeding = false;
        IsBlinded = false;
        IsConfused = false;
        IsCrippled = false;
        IsExhausted = false;
        IsStunned = false;

        if (ArmorBuffDebuff < 0)
        { ArmorBuffDebuff = 0; }
        if (DamageBuffDebuff < 0)
        { DamageBuffDebuff = 0; }
    }

    public void Revive() // Revives an instamce
    {
        if (IsKo)
        {
            IsKo = false;
            Hp = MaxHp;
            Mana = MaxMana;
            AilmentClearance();
            GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().DeathOrRevive();
        }
    }

    public void ApplyAoE(string AoEStyle, float value) // Apllies all kinds of AoE Effects
    {
        List<Entity> AlliesList = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetPlayers();
        List<Entity> EnemiesList = GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetNPCs();

        switch (AoEStyle)
        {
            case "Damage":
                foreach (Entity target in EnemiesList)
                { Attack(target, (int)value); }
                break;
            case "Bleed":
                foreach (Entity target in EnemiesList)
                { target.IsBleeding = true; }
                break;
            case "Stun":
                foreach (Entity target in EnemiesList)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (Random.Range(0.00f, 1f) <= value)
                        { GameObject.Find("Game Manager Battle").GetComponent<PhotonView>().RPC("RPC_Stun", RpcTarget.All, target.EntityID); }
                    }
                }
                break;
            case "Damage Buff2":
                foreach (Entity target in EnemiesList)
                { target.ReceiveDamageBuffDebuff(value); }
                break;

            case "Heal":
                foreach (Entity target in AlliesList)
                { target.Heal((int)value); }
                break;
            case "Damage Buff":
                foreach (Entity target in AlliesList)
                { target.ReceiveDamageBuffDebuff(value); }
                break;
            case "Damage2":
                foreach (Entity target in AlliesList)
                { Attack(target, (int)value); }
                break;
        }
    }

    public void HealAndManaRegenFromMinigame(int heal, int mana)
    {
        if (Hp != 0)
        { Hp += heal; }

        if (Hp > MaxHp)
        { Hp = MaxHp; }

        Mana += mana;

        if (Mana > MaxMana)
        { Mana = MaxMana; }
    }
    #endregion
}
