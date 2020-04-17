using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TargetStyle
{
    BaseAttack, ShowSelf, Self, Teammates, AoETeammates, Enemies, AoEEnemies, Revive, 지영아, Karim, Default
}

public class EntityInfoFieldManager : MonoBehaviour {

    public Entity myEntity;

    [SerializeField] private Text NameField;

    [SerializeField] private RectTransform HealthBar;
    [SerializeField] private RectTransform ManaBar;
    [SerializeField] private RectTransform TargetableBackground;
    [SerializeField] private GameObject TargetableButton;
    [SerializeField] private Text HealthText;
    [SerializeField] private Text ManaText;

    [SerializeField] private GameObject AttackRecapField;
    [SerializeField] private Text AttackRecap;

    public int ListID;

    public GameObject IconBleed;
    public GameObject IconStun;
    public GameObject IconConfusion;
    public GameObject IconBlinded;
    public GameObject IconExhausted;
    public GameObject IconRiposte;
    public GameObject IconCrippled;

    private Color grey = new Color(0.70f, 0.75f, 0.71f, 1f);
    private Color blue = new Color(0.20f, 0.87f, 1.00f, 1f);
    private Color spring_green = new Color(0.20f, 1.00f, 0.60f, 1f);
    private Color green = new Color(0.20f, 1.00f, 0.33f, 1f);
    private Color lavendar = new Color(0.45f, 0.31f, 0.59f, 1f);
    private Color red = new Color(0.90f, 0.00f, 0.30f, 1f);
    private Color crimson = new Color(0.69f, 0.00f, 0.16f, 1f);
    private Color aztec_gold = new Color(0.76f, 0.60f, 0.33f, 1f);
    private Color cherry = new Color(1.00f, 0.72f, 0.77f, 1f);
    private Color orange = new Color(1.00f, 0.40f, 0.10f, 1f);


    // Start is called before the first frame update
    void Start()
    {
        IconBleed.SetActive(false);
        IconStun.SetActive(false);
        IconConfusion.SetActive(false);
        IconBlinded.SetActive(false);
        IconExhausted.SetActive(false);
        IconRiposte.SetActive(false);
        IconCrippled.SetActive(false);

        TargetableButton.SetActive(false);
        AttackRecapField.SetActive(false);
    }

    public void SetNameAndEntity (string name, Entity entity)
    {
        myEntity = entity;
        NameField.text = name + " - " + myEntity.TypeName;
    }

    public void UpdateOverview()
    {
        float HealthRatio = Mathf.Max(0f, ((float) myEntity.Hp) / ((float) myEntity.MaxHp)); //Health
        Vector3 scaleHealthBar = HealthBar.localScale;
        scaleHealthBar.x = HealthRatio;
        HealthBar.localScale = scaleHealthBar;
        HealthText.text = myEntity.Hp.ToString();

        float ManaRatio = Mathf.Max(0f, ((float)myEntity.Mana) / ((float)myEntity.MaxMana)); //Mana
        Vector3 scaleManaBar = ManaBar.localScale;
        scaleManaBar.x = ManaRatio;
        ManaBar.localScale = scaleManaBar;
        ManaText.text = myEntity.Mana.ToString();

        IconBleed.SetActive(myEntity.IsBleeding);
        IconStun.SetActive(myEntity.IsStunned);
        IconConfusion.SetActive(myEntity.IsConfused);
        IconBlinded.SetActive(myEntity.IsBlinded);
        IconExhausted.SetActive(myEntity.IsExhausted);
        IconRiposte.SetActive(myEntity.IsRiposte);
        IconCrippled.SetActive(myEntity.IsCrippled);
    }

    public void ApplyTargetable(TargetStyle target, Entity SourceEntity) // All target Styles
    {
        TargetableButton.SetActive(false);
        TargetableBackground.GetComponent<RawImage>().color = grey;

        if (myEntity.IsKo && target != TargetStyle.Revive)
        { return; }

        switch (target)
        {
            case TargetStyle.BaseAttack: //
                if (myEntity is Player)
                {
                    TargetableBackground.GetComponent<RawImage>().color = lavendar;
                    TargetableButton.SetActive(true);
                }
                else
                {
                    TargetableBackground.GetComponent<RawImage>().color = red;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.ShowSelf:
                if (myEntity.Type == SourceEntity.Type)
                {
                    TargetableBackground.GetComponent<RawImage>().color = lavendar;
                }
                break;
            case TargetStyle.Self:
                if (myEntity.Type == SourceEntity.Type)
                {
                    TargetableBackground.GetComponent<RawImage>().color = blue;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.Teammates:
                if (myEntity is Player)
                {
                    TargetableBackground.GetComponent<RawImage>().color = green;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.AoETeammates:
                if (myEntity is Player)
                {
                    TargetableBackground.GetComponent<RawImage>().color = spring_green;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.Enemies:
                if (myEntity is NPC)
                {
                    TargetableBackground.GetComponent<RawImage>().color = red;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.AoEEnemies:
                if (myEntity is NPC)
                {
                    TargetableBackground.GetComponent<RawImage>().color = crimson;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.Revive:
                if (myEntity is Player && myEntity.IsKo)
                {
                    TargetableBackground.GetComponent<RawImage>().color = aztec_gold;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.지영아:
                if (myEntity is Priestress)
                {
                    TargetableBackground.GetComponent<RawImage>().color = cherry;
                    TargetableButton.SetActive(true);
                }
                if (myEntity is NPC)
                {
                    TargetableBackground.GetComponent<RawImage>().color = red;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.Karim:
                if (myEntity is Crusader)
                {
                    TargetableBackground.GetComponent<RawImage>().color = cherry;
                    TargetableButton.SetActive(true);
                }
                break;
            case TargetStyle.Default:
                TargetableBackground.GetComponent<RawImage>().color = orange;
                break;
        }
    }

    public void SetAttackRecap(string attackRecap) // Attack Recap Field is turnt on
    {
        AttackRecapField.SetActive(true);
        AttackRecap.text = attackRecap;
    }

    public void RemoveAttackRecap()
    {
        AttackRecapField.SetActive(false); // Attack Recap Field is turnt on
    }
}
