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
    };

    List<TargetStyle> AttackTargets = new List<TargetStyle>()
    {
        TargetStyle.Enemies,
    };
    #endregion

    #region Constructor
    public JackOLantern(int entityID)
            : base(entityID, EntityType.JackOLantern, "Jack-O-Lantern", 200, 10, 0.25f, 1)
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
        }
    }
    #endregion
}
#endregion