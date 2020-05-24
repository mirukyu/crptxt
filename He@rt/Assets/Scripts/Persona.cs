using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Attack Methods that may be used:
 * 1) applying any kind of ailment on anyone
 * 2) Dmg Buff / Debuff +++ Armor Buff / Debuff +++ Evasiveness
 * 3) Dealing dmg
 * 4) Mana has yet to be implemented
 * 
 * General Structure:
 * 1) Setup: Attack names, Attack descriptions, Attack Details, Mana Requirement, Attack targets (All Lists)
 * 2) Constructor
 * 3) Methods: Get Attack Name, Get Attack Description, Get Attack Detail, Get Mana Requirement, Get Attack Target, Use Spell
 * 
 * => Aramusha, Priestress, Mage, Crusader
 */

#region Aramusha
public class Aramusha : Player                                                                     // ARAMUSHA //
{
    #region Setup
    List<string> AttackNames = new List<string>()
    {
        "Stardust Stream",
        "Zenshin School",
        "Water Flow",
        "Blood Stained",
        "Kabe Don",
        "Stance"
    };

    List<string> AttackDescriptions = new List<string>()
    {
        "A 27 -hit combo in which the character goes full anime. Not inspired by any particular anime btw. Marks will be laid upon soul and body.", //Charge for one turn
        "The only thing sharper than the blade is the mind. Deal damage and receive a damgae buff thanks to those beautifully drawn jaws, like Jeez.",
        "A plethora of attacks the flow as smooth as the ikemen voice.",
        "Edgy as fuck, rage on with a strike meant to shed blood.",
        "Charm the enemies or a particular ally, and make them either not be able to attack you, or make them fall for you some more cuz y not.",
        "Enter riposte state - get hit, hit back, for 40% of your force."
    };

    List<string> AttackDetails = new List<string>()
    {
        "Target: Enemy \n Damage: 100 \n Becomes Crippled \n Cripple: 10%",
        "Target: Enemy \n Damage: 20 \n\n Damage Buff: +30%",
        "Target: AoE Enemies \n Damage: 25",
        "Target: Enemy \n Damage: 15 \n Target: AoE Enemies \n Appllies Bleed",
        "Target: Priestress \n Spell Boost: 140% \n Target: Enemy \n Increase Own Dodge Rate: +15%",
        "Target: Self \n Activates Riposte \n Riposte: 40%"
    };

    List<int> ManaRequirement = new List<int>()
    {
        30,
        15,
        25,
        10,
        0,
        15
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Enemies,
        TargetStyle.Enemies,
        TargetStyle.AoEEnemies,
        TargetStyle.AoEEnemies,
        TargetStyle.지영아,
        TargetStyle.Self
    };
    #endregion

    #region Constructor
    public Aramusha(string username, int entityID)
            : base(username, entityID, EntityType.Aramusha, "Aramusha", 60, 115, 25, 0.3f, 6)
    { }
    #endregion

    #region Methods
    public string GetAttackName (int index)
    { return AttackNames[index]; }

    public string GetAttackDescription (int index)
    { return AttackDescriptions[index]; }

    public string GetAttackDetail (int index)
    { return AttackDetails[index]; }

    public int GetManaRequirement (int index)
    { return ManaRequirement[index]; }

    public TargetStyle GetAttackTarget (int index)
    { return AttackTargets[index]; }

    public void UseSpell (int index, Entity target)
    {
        SpendMana(ManaRequirement[index]);

        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Persona");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Persona");

        switch (index)
        {
            case 0:
                Attack(target, 100);
                IsCrippled = true;
                CrippledPercentage = 0.10f;            
                break;
            case 1:
                Attack(target, 20);
                ReceiveDamageBuffDebuff(0.3f);
                break;
            case 2:
                ApplyAoE("Damage", 25);
                break;
            case 3:
                Attack(target, 15);
                ApplyAoE("Bleed", 0);
                break;
            case 4:
                if (target is Priestress)
                { ((Priestress)target).SpellBoost = 1.4f; }
                else
                { DodgeRate = BaseDodgeRate + 0.15f; }
                break;
            case 5:
                IsRiposte = true;
                RipostePercentage = 0.4f;
                break;
        }
    }
    #endregion
}
#endregion

#region Priestress
public class Priestress : Player                                                                   // PRIESTRESS //
{
    #region Setup
    private float spellBoost;

    public float SpellBoost
    {
        get { return spellBoost; }
        set { spellBoost = value; }
    }

    List<string> AttackNames = new List<string>()
    {
        "Black Swan",
        "Swan Feather",
        "White Breath",
        "Bestow Life",
        "Relish in Grace",
        "EXPLOSION"
    };

    List<string> AttackDescriptions = new List<string>()
    {
        "Hidden talents will be unleashed, taken enemies aback: Healers do deal damage.",
        "Regenerates Mana of a fellow bretheren.",
        "Clear all ailments off of one seemingly very unlucky soul (bonus package: get a life).",
        "No Life? No Friends? Here's the solution: Heal all of y'all!.",
        "With thy Grace, revive thee who put aside his own life for others.",
        "Totally original, unrigged, innovative move: BLOWS UP the enemies." // put the stack here
    };

    List<string> AttackDetails = new List<string>()
    {
        "Target: Enemy \n Damage: 30",
        "Target: Teammate \n Mana: +60 \n\n Spell Boost applicable",
        "Target: Teammate \n Heal: +20 \n Ailment Clearance \n Spell Boost applicable",
        "Target: AoE Teammates \n Heal: +40 \n\n Spell Boost applicable",
        "Target: Teammate \n Revive \n (Target must be KO)",
        "Target: AoE Enemies \n Damage: 50"
    };

    List<int> ManaRequirement = new List<int>()
    {
        10,
        10,
        15,
        40,
        100,
        30
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Enemies,
        TargetStyle.Teammates,
        TargetStyle.Teammates,
        TargetStyle.AoETeammates,
        TargetStyle.Revive,
        TargetStyle.AoEEnemies
    };
    #endregion

    #region Constructor
    public Priestress(string username, int entityID)
            : base(username, entityID, EntityType.Priestess, "Priestess", 100, 200, 15, 0.1f, 6)
    { spellBoost = 1f; }
    #endregion

    #region Methods
    public string GetAttackName (int index)
    { return AttackNames[index]; }

    public string GetAttackDescription (int index)
    { return AttackDescriptions[index]; }

    public string GetAttackDetail (int index)
    { return AttackDetails[index]; }

    public int GetManaRequirement(int index)
    { return ManaRequirement[index]; }

    public TargetStyle GetAttackTarget (int index)
    { return AttackTargets[index]; }

    public void UseSpell(int index, Entity target)
    {
        SpendMana(ManaRequirement[index]);

        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Persona");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Persona");

        switch (index)
        {
            case 0:
                Attack(target, 30);
                break;
            case 1:
                target.RegenerateMana((int)(60 * SpellBoost));
                break;
            case 2:
                target.Heal((int)(20 * SpellBoost));
                target.AilmentClearance();
                break;
            case 3:
                ApplyAoE("Heal", 40 * SpellBoost);
                break;
            case 4:
                target.Revive();
                break;
            case 5:
                ApplyAoE("Damage", 50);
                break;
        }
    }
    #endregion
}
#endregion

#region Mage
public class Mage : Player                                                                         // MAGE //
{
    #region Setup
    private float spellBoost;

    public float SpellBoost
    {
        get { return spellBoost; }
        set { spellBoost = value; }
    }

    List<string> AttackNames = new List<string>()
    {
        "Your Code is Trash",
        "Effective Malware",
        "Optimization",
        "Decipher",
        "Hayawan",
        "Ad Blocker"
    };

    List<string> AttackDescriptions = new List<string>()
    {
        "Hacking into the enemy, successfully hurts their self esteem (and damage output).",
        "Corrupting the files of the enemy, hampers their ability to function and defend.",
        "Using functions that are not allowed by EPITA, optimizes the party’s code.",
        "Decrypting the enemy’s system, harms them on a personal basis.",
        "Insulting a very 'special friend', gets invigorated and gets motivated to get this shit done.",
        "Installs and launches defensive systems that guard a comrade."
    };

    List<string> AttackDetails = new List<string>()
    {
        "Target: Enemy \n Damage Debuff: 20% \n\n Spell Boost applicable",
        "Target: Enemy \n Armor Debuff: 40% \n\n Spell Boost applicable",
        "Target: AoE Teammates \n Damage Buff: 20% \n\n Spell Boost applicable",
        "Target: Enemy \n Damage: 35",
        "Target: Crusader \n Own Spell Boost: 150%",
        "Target: Teammate \n Armor Buff: 20% \n\n Spell Boost applicable"
    };

    List<int> ManaRequirement = new List<int>()
    {
        10,
        30,
        50,
        15,
        10,
        15
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Enemies,
        TargetStyle.Enemies,
        TargetStyle.AoETeammates,
        TargetStyle.Enemies,
        TargetStyle.Karim,
        TargetStyle.Teammates
    };
    #endregion

    #region Constructor
    public Mage(string username, int entityID)
            : base(username, entityID, EntityType.Mage, "Mage", 125, 150, 15, 0f, 6)
    { spellBoost = 1f; }
    #endregion

    #region Methods
    public string GetAttackName (int index)
    { return AttackNames[index]; }

    public string GetAttackDescription (int index)
    { return AttackDescriptions[index]; }

    public string GetAttackDetail (int index)
    { return AttackDetails[index]; }

    public int GetManaRequirement(int index)
    { return ManaRequirement[index]; }

    public TargetStyle GetAttackTarget (int index)
    { return AttackTargets[index]; }

    public void UseSpell(int index, Entity target)
    {
        SpendMana(ManaRequirement[index]);

        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Persona");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Persona");

        switch (index)
        {
            case 0:
                target.ReceiveDamageBuffDebuff((-0.2f) * SpellBoost);
                break;
            case 1:
                target.ReceiveArmorBuffDebuff((-0.4f) * SpellBoost);
                break;
            case 2:
                ApplyAoE("Damage Buff", 0.2f * SpellBoost);
                break;
            case 3:
                Attack(target, 35);
                break;
            case 4:
                SpellBoost = 1.5f;
                break;
            case 5:
                target.ReceiveArmorBuffDebuff(0.2f * SpellBoost);
                break;
        }
    }
    #endregion
}
#endregion

#region Crusader
public class Crusader : Player                                                                      // CRUSADER //
{
    #region Setup
    List<string> AttackNames = new List<string>()
    {
        "Fo-getit",
        "Pillar of Light",
        "Depresso",
        "Hollow",
        "OathKeeper",
        "Sanctuary"
    };

    List<string> AttackDescriptions = new List<string>()
    {
        "Lacks the memory space to allocate for 'ouch', ignores damage.",
        "Standing ardently to Praise The SUUUUUUUUNNNNNN, blinds the enemy with supremacy.",
        "Gushing out thy thoughts, the enemy starts questioning life and their existence ... Why am I here?",
        "Flagellation brings forth absolution. Hollowness is worse than the darkest abyss.",
        "'Never abandon thy companions; never lose thyself in the eye of despair' ~ A promise revisited, and a duty to be kept.",
        "A Babylon of your making."
    };

    List<string> AttackDetails = new List<string>()
    {
        "Target: Self \n Increase Dodge Rate: +30%",
        "Target: AoE Enemies \n 60% Chance to Apply Stun",
        "Target: Enemy \n Applies Blind \n Applies Confused",
        "Target: AoE Enemies \n Damage: 50 \n\n (Unlock: Team HP < 35%)",
        "Target: Enemy \n Damage: 20 \n Activates Riposte \n Riposte: 40% \n (Unlock: Team HP < 35%)",
        "Target: Enemy \n Damage: 80 \n\n (Unlock: Team HP < 35%)"
    };

    List<int> ManaRequirement = new List<int>()
    {
        15,
        30,
        25,
        15,
        15,
        20
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Self,
        TargetStyle.AoEEnemies,
        TargetStyle.Enemies,
        TargetStyle.AoEEnemies,
        TargetStyle.Enemies,
        TargetStyle.Enemies
    };
    #endregion

    #region Constructor 
    public Crusader(string username, int entityID)
            : base(username, entityID, EntityType.Crusader, "Crusader", 250, 150, 20, 0f, 3)
    { }
    #endregion

    #region Methods
    public string GetAttackName (int index)
    { return AttackNames[index]; }

    public string GetAttackDescription (int index)
    { return AttackDescriptions[index]; }

    public string GetAttackDetail (int index)
    { return AttackDetails[index]; }

    public int GetManaRequirement(int index)
    { return ManaRequirement[index]; }

    public TargetStyle GetAttackTarget (int index)
    { return AttackTargets[index]; }

    public void CheckTeamHealth() // Verifies Team Health level (to unblock the last three attacks)
    {
        if (GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetTeamHealthLevel() < 0.35f)
        { AmountAttacks = 6; }
        else
        { AmountAttacks = 3; }
    }

    public void UseSpell(int index, Entity target)
    {
        SpendMana(ManaRequirement[index]);

        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Persona");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Persona");

        switch (index)
        {
            case 0:
                DodgeRate = 0.3f;
                break;
            case 1:
                ApplyAoE("Stun", 0.6f);
                break;
            case 2:
                target.IsConfused = true;
                target.IsBlinded = true;
                break;
            case 3:
                ApplyAoE("Damage", 50);
                break;
            case 4:
                IsRiposte = true;
                RipostePercentage = 0.35f;
                Attack(target, 20);
                break;
            case 5:
                Attack(target, 80);
                break;
        }
    }
    #endregion
}
#endregion