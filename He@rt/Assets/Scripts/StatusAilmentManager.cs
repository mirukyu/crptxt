using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AilmentType
{ Bleed, Stun, Confusion, Blinded, Exhausted, Riposte, Crippled }

public class StatusAilmentManager : MonoBehaviour {

    [SerializeField] private AilmentType Ailment = AilmentType.Bleed;
    string ailmentDescription;

    public void Start() // Takes respective description for each ailment
    {
        switch (Ailment)
        {
            case AilmentType.Bleed:
                ailmentDescription = "HP will be lost over time";
                break;
            case AilmentType.Stun:
                ailmentDescription = "Next turn no action will be possible";
                break;
            case AilmentType.Confusion:
                ailmentDescription = "Attack will be randomized";
                break;
            case AilmentType.Blinded:
                ailmentDescription = "Target will be randomized";
                break;
            case AilmentType.Exhausted:
                ailmentDescription = "No special skills can be used this turn";
                break;
            case AilmentType.Riposte:
                ailmentDescription = "A percentage of received damage will be reflected to the attacker.";
                break;
            case AilmentType.Crippled:
                ailmentDescription = "A percentage of outgoing damage will be inflicted on oneself";
                break;
        }
    }

    public void CursorEnter() // Verifies whether mouse enters the ailment field
    {
        GameObject.Find("Game Manager Battle").GetComponent<TextManager>().ActivateCursorTextFieldAilment(Ailment.ToString(), ailmentDescription);
    }

    public void CursorExit() // Verifies whether mouse exits the ailment field
    {
        GameObject.Find("Game Manager Battle").GetComponent<TextManager>().DesactivateCursorTextField();
    }
}