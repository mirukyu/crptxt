using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    [SerializeField] private Text TextFieldMain;
    [SerializeField] private Text TextFieldMinor;

    [SerializeField] private GameObject AttackButtonField;
    [SerializeField] private GameObject ActionsButtonField;

    [SerializeField] private GameObject AttackField1;
    [SerializeField] private GameObject AttackField2;
    [SerializeField] private GameObject AttackField3;
    [SerializeField] private GameObject AttackField4;
    [SerializeField] private GameObject AttackField5;
    [SerializeField] private GameObject AttackField6;

    [SerializeField] private GameObject CursorTextField;
    [SerializeField] private Text CursorTextFieldName;
    [SerializeField] private Text CursorTextFieldDescription;

    [SerializeField] private GameObject Barrier;

    private bool CursorTextFieldActive = false;
    private List<string> AttackDetails = new List<string>() { };

    // Start is called before the first frame update
    void Start()
    {
        OutputTextMain("");

        AttackButtonField.SetActive(false);
        ActionsButtonField.SetActive(true);

        CursorTextField.SetActive(false);

        Barrier.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (CursorTextFieldActive) // Taking care of the cursor description field
        {
            CursorTextField.transform.position = Input.mousePosition;

            if (Input.mousePosition.x < Screen.width * 0.8f) // Anchor left or right
            {
                CursorTextField.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                CursorTextField.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                CursorTextField.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            }
            else
            {
                CursorTextField.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                CursorTextField.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
                CursorTextField.GetComponent<RectTransform>().pivot = new Vector2(1, 1);
            }
        }
    }

    public void OutputTextMain(string TextToOutput) // Text on main output
    { TextFieldMain.text = TextToOutput; }

    public void OutputTextMinor(string TextToOutput) // Text on minor output
    { TextFieldMinor.text = TextToOutput; }

    public void FillInAttackFields(Entity SourceEntity) // Adapts Attack Fields according to each payer
    {
        OutputTextMain("");
        AttackDetails = new List<string> { };
        GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Default, null);

        GameObject[] AttackFieldList = new GameObject[] { AttackField1, AttackField2, AttackField3, AttackField4, AttackField5, AttackField6 };
        AttackField1.SetActive(false);
        AttackField2.SetActive(false);
        AttackField3.SetActive(false);
        AttackField4.SetActive(false);
        AttackField5.SetActive(false);
        AttackField6.SetActive(false);

        if (SourceEntity == null)
        { return; }

        if (SourceEntity is Crusader)
        { ((Crusader)SourceEntity).CheckTeamHealth(); }

        for (int i = 0; i < ((Player)SourceEntity).AmountAttacks; i++)
        {
            AttackFieldList[i].SetActive(true);
            AttackFieldList[i].GetComponent<AttackButtonManager>().OutputAttackName(((Player) SourceEntity).GetAttackNameGeneral(i, SourceEntity));
            AttackFieldList[i].GetComponent<AttackButtonManager>().OutputAttackDescription(((Player)SourceEntity).GetAttackDescriptionGeneral(i, SourceEntity));
            AttackFieldList[i].GetComponent<AttackButtonManager>().OutputManaRequirement(((Player) SourceEntity).GetManaRequirementGeneral(i, SourceEntity));

            AttackDetails.Add(((Player)SourceEntity).GetAttackNameGeneral(i, SourceEntity));
            AttackDetails.Add(((Player)SourceEntity).GetAttackDetailsGeneral(i, SourceEntity));
        }
    }

    public void ActivateCursorTextFieldAilment (string ailmentName, string ailmentDescription) // Cursor Text Field is turnt on (Status Ailment)
    {
        CursorTextFieldActive = true;
        CursorTextField.SetActive(true);

        CursorTextFieldName.text = ailmentName;
        CursorTextFieldDescription.text = ailmentDescription;
    }

    public void ActivateCursorTextFieldAttackDetail (int i) // Cursor Text Field is turnt on (Attack Details)
    {
        CursorTextFieldActive = true;
        CursorTextField.SetActive(true);

        CursorTextFieldName.text = AttackDetails[2*i];
        CursorTextFieldDescription.text = AttackDetails[2*i + 1];
    }

    public void DesactivateCursorTextField() // Cursor Text Field is turnt off
    {
        CursorTextFieldActive = false;
        CursorTextField.SetActive(false);
    }

    public void PressButton(string ButtonFunction) //Buttons in Action Select
    {
        switch (ButtonFunction)
        {
            case "Return":
                BackToActionSelect();
                GetComponent<Combat>().Reset();
                GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Default, null);
                break;
            case "Attack":
                GetComponent<Combat>().ChooseAttack(7);
                //PressButton("Spell");
                break;
            case "Spell":
                AttackButtonField.SetActive(true);
                ActionsButtonField.SetActive(false);
                OutputTextMinor(GetComponent<EntityCreation>().EntityList[CharacterSetUp.Game.myID].TypeName + " =>");
                GetComponent<EntityCreation>().ApplyTargetable(TargetStyle.Default, null);
                break;
            case "Flee":
                if (Random.Range(0f, 1f) <= 0.8f * GameObject.Find("Game Manager Battle").GetComponent<EntityCreation>().GetTeamHealthLevel())
                { OutputTextMinor("You could successfully flee!"); }
                else
                { OutputTextMinor("Failure! Keep fighting you coward!"); }
                break;
            case "Play Random":
                GetComponent<AudioManager>().PlayRandom();
                break;
        }
    }

    public void BackToActionSelect() // Back to Action select after carrying out an action
    {
        AttackButtonField.SetActive(false);
        ActionsButtonField.SetActive(true);
        OutputTextMinor("Fall the decision ought to bring destruction upon your foes.");

        RemoveBarrier();
    }

    public void RemoveBarrier()
    { Barrier.SetActive(false); }
    public void SetBarrier()
    { Barrier.SetActive(true); }
}
