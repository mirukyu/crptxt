using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour {

    [SerializeField] private RectTransform TargetableBackground;
    [SerializeField] private GameObject TargetableButton;
    [SerializeField] private Text NameField;
    [SerializeField] private EntityType Character = EntityType.Aramusha;

    public int ListID;

    public bool AlreadySelected = false;
    public bool Selected = false;
    public bool SelectedByOtherPlayer = false;

    private Color grey = new Color(0.70f, 0.75f, 0.71f, 1f);
    private Color green = new Color(0.20f, 1.00f, 0.33f, 1f);
    private Color blue = new Color(0.20f, 0.87f, 1.00f, 1f);
    private Color lavendar = new Color(0.45f, 0.31f, 0.59f, 1f);

    // Use this for initialization
    void Start ()
    {
        NameField.text = Character.ToString();
    }

    public void ApplySelectionColour()
    {
        TargetableButton.SetActive(false);
        TargetableBackground.GetComponent<RawImage>().color = grey;

        if (!AlreadySelected)
        {
            TargetableButton.SetActive(true);
            if (Selected)
            { TargetableBackground.GetComponent<RawImage>().color = blue; }
            else
            { TargetableBackground.GetComponent<RawImage>().color = green; }

            if (SelectedByOtherPlayer)
            {
                TargetableButton.SetActive(false);
                TargetableBackground.GetComponent<RawImage>().color = lavendar;
            }
        }
    }

    public void Select()
    { Selected = true; }

    public void Deselect()
    { Selected = false; }

    public void Selecting()
    { AlreadySelected = true; }

    public EntityType GetCharacter()
    { return Character; }
}
