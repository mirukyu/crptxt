using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackDetailFieldManager : MonoBehaviour {

    [SerializeField] private int index;

    public void CursorEnter() // Verifies whether mouse enters the ailment field
    {
        GameObject.Find("Game Manager Battle").GetComponent<TextManager>().ActivateCursorTextFieldAttackDetail(index);

    }

    public void CursorExit() // Verifies whether mouse exits the ailment field
    {
        GameObject.Find("Game Manager Battle").GetComponent<TextManager>().DesactivateCursorTextField();
    }
}
