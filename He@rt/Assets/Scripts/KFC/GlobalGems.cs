using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalGems : MonoBehaviour {

    public static int karinCount;
    public GameObject KarinDisplay;
    public int internalKarin;

    public void Start()
    { karinCount = 0; }

    // Update is called once per frame
    void Update () {
        internalKarin = karinCount;
        KarinDisplay.GetComponent<Text>().text = "Karins : " + internalKarin;
	}
}
