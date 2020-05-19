using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainbuttonclick : MonoBehaviour {

    public GameObject textbox;

    public void ClickButton()
    {
        textbox.SetActive(true);
        GlobalGems.karinCount += 1;
    }
}
