using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreCanvasManager : MonoBehaviour {

    [SerializeField] private GameObject NextButton;
    [SerializeField] private GameObject PreviosuButton;

    [SerializeField] private GameObject[] CharacterList;
    private int index = 0;

    private void Start()
    {
        foreach (GameObject e in CharacterList)
            e.SetActive(false);

        CharacterList[0].SetActive(true);
        PreviosuButton.SetActive(false);
        NextButton.SetActive(true);
    }

    public void NextButtonClick()
    {
        PreviosuButton.SetActive(true);

        CharacterList[index].SetActive(false);
        index++;
        CharacterList[index].SetActive(true);

        if (index == CharacterList.Length - 1)
            NextButton.SetActive(false);
    }

    public void PreviosuButtonClick()
    {
        NextButton.SetActive(true);

        CharacterList[index].SetActive(false);
        index--;
        CharacterList[index].SetActive(true);

        if (index == 0)
            PreviosuButton.SetActive(false);
    }

}
