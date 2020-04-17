using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonManager : MonoBehaviour
{

    [SerializeField] private Text AttackName;
    [SerializeField] private Text AttackDescription;
    [SerializeField] private Text ManaField;

    public void OutputAttackName(string attackName)
    { AttackName.text = attackName; }

    public void OutputAttackDescription(string attackDescription)
    { AttackDescription.text = attackDescription; }

    public void OutputManaRequirement(int manaAmount)
    { ManaField.text = manaAmount.ToString(); }
}
