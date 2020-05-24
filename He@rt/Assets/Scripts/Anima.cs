using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* General Structure:
 * 1) Setup: Attack names, Attack descriptions, Attack targets
 * 2) Constructor
 * 3) Methods: Get Attack Name, Get Attack Target, Use Spell
 *
 * => Jack-O-Lantern
 */

#region Jack-O-Lantern
public class JackOLantern : NPC                                                                    // Jack-O-Lantern //
{
    #region Setup
    List<string> AttackNames = new List<string>()
    {
        "Flame of Despair",
        "Pumpkin Pump",
        "Spook Up"
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Enemies,
        TargetStyle.Enemies,
        TargetStyle.AoETeammates 
    };

    #endregion

    #region Constructor
    public JackOLantern(int entityID)
            : base(entityID, EntityType.JackOLantern, "Jack-O-Lantern", 150, 10, 0.15f, 3)
    { }
    #endregion

    #region Methods
    public string GetAttackName(int index)
    { return AttackNames[index]; }

    public TargetStyle GetAttackTarget(int index)
    { return AttackTargets[index]; }

    public void UseSpell(int index, Entity target)
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Anima");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Anima");

        switch (index)
        {
            case 0:
                Attack(target, 10);
                break;

            case 1:
                target.ReceiveArmorBuffDebuff(-0.2f); 
                break;

            case 2:
                ApplyAoE("Damage Buff2", 0.15f); 
                break;
        }
    }

    #endregion
}
#endregion

#region Skelly
public class Skelly : NPC                                                                    // Jack-O-Lantern //
{
    #region Setup
    List<string> AttackNames = new List<string>()
    {
        "Bone Removal",
        "Exoskeleton",
        "Fasten Your Seatbelt"
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Enemies,
        TargetStyle.Self, 
        TargetStyle.Enemies
    };
    #endregion

    #region Constructor
    public Skelly(int entityID)
            : base(entityID, EntityType.Skelly, "Skelly", 100, 10, 0f, 3)
    { }
    #endregion

    #region Methods
    public string GetAttackName(int index)
    { return AttackNames[index]; }

    public TargetStyle GetAttackTarget(int index)
    { return AttackTargets[index]; }

    public void UseSpell(int index, Entity target)
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Anima");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Anima");

        switch (index)
        {
            case 0:
                Attack(target, 5);
                target.IsBleeding = true;
                target.IsCrippled = true;
                target.CrippledPercentage = 0.05f; 
                break;

            case 1:
                IsRiposte = true;
                RipostePercentage = 0.10f; 
                Heal(15);
                break; 

            case 2:
                target.IsStunned = true; 
                break; 
        }
    }
    #endregion
}
#endregion

#region WispBlue 
public class WispBlue : NPC                                                                    // Jack-O-Lantern //
{
    #region Setup
    List<string> AttackNames = new List<string>()
    {
        "Soul Dismissal",
        "Purgatory",
        "Ghost Gospel"
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Enemies,
        TargetStyle.AoEEnemies, 
        TargetStyle.Enemies
    };
    #endregion

    #region Constructor
    public WispBlue(int entityID)
            : base(entityID, EntityType.WispBlue, "Blue Wisp", 10, 10, 0.5f, 3)
    { }
    #endregion

    #region Methods
    public string GetAttackName(int index)
    { return AttackNames[index]; }

    public TargetStyle GetAttackTarget(int index)
    { return AttackTargets[index]; }

    public void UseSpell(int index, Entity target)
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Anima");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Anima");

        switch (index)
        {
            case 0:
                Attack(target, 5);
                target.IsExhausted = true;
                target.SpendMana(10); 
                break;

            case 1:
                ApplyAoE("Damage2", 10); 
                break;

            case 2:
                Attack(target, 20); 
                break; 
        }
    }
    #endregion
}
#endregion 

#region WispRed
public class WispRed : NPC                                                                    // Jack-O-Lantern //
{
    #region Setup
    List<string> AttackNames = new List<string>()
    {
        "Inflammation",
        "Fire Squad",
        "Blaze of Glory"
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.AoEEnemies,
        TargetStyle.Teammates,
        TargetStyle.Enemies
    };
    #endregion

    #region Constructor
    public WispRed(int entityID)
            : base(entityID, EntityType.WispRed, "Red Wisp", 75, 10, 0.1f, 3)
    { }
    #endregion

    #region Methods
    public string GetAttackName(int index)
    { return AttackNames[index]; }

    public TargetStyle GetAttackTarget(int index)
    { return AttackTargets[index]; }

    public void UseSpell(int index, Entity target)
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Anima");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Anima");

        switch (index)
        {
            case 0:
                ApplyAoE("Damage2", 10); 
                break;

            case 1:
                target.DodgeRate = target.BaseDodgeRate + 0.10f; 
                break;

            case 2:
                Attack(target, 20);
                break;
        }
    }
    #endregion
}
#endregion 

#region BunBun
public class BunBun : NPC                                                                    // Jack-O-Lantern //
{
    #region Setup
    List<string> AttackNames = new List<string>()
    {
        "Bunny Charm",
        "Bestial Instinct",
        "Hop'n Bite"
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Enemies,
        TargetStyle.Self,
        TargetStyle.Enemies
    };
    #endregion

    #region Constructor
    public BunBun(int entityID)
            : base(entityID, EntityType.BunBun, "Bun Bun", 400, 10, 0.05f, 3) 
    { }
    #endregion

    #region Methods
    public string GetAttackName(int index)
    { return AttackNames[index]; }

    public TargetStyle GetAttackTarget(int index)
    { return AttackTargets[index]; }

    public void UseSpell(int index, Entity target)
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(AttackNames[index], "Anima");
        GameObject.Find("Particle Manager").GetComponent<ParticleManager>().CreateParticle(AttackNames[index], target, "Anima");

        switch (index)
        {
            case 0:
                target.IsConfused = true;
                target.IsBlinded = true; 
                break;

            case 1:
                ReceiveDamageBuffDebuff(0.5f); 
                break;

            case 2:
                Attack(target, 50); 
                break;
        }
    }
    #endregion
}
#endregion