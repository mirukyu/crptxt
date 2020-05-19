using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour {

    
    public GameObject Emerald;
    public GameObject Ruby;
    public GameObject Saphire;
    public GameObject Diamond;
    public GameObject Good;
    public GameObject Great;
    public GameObject Ultima;
    public GameObject t1;
    public GameObject t2;
    public GameObject t3;
    public GameObject t4;
    public GameObject t5;
    public GameObject t6;
    public GameObject t7;
    public GameObject t8;
    public GameObject t9;
    public GameObject t10;
    public GameObject t11;
    public GameObject t12;
    public GameObject t13;

    /*public Sprite Emerald;
    public Sprite Ruby;
    public Sprite Saphire;
    public Sprite Diamond;
    public Sprite Good;
    public Sprite Great;
    public Sprite Ultima;*/



    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
    }

    public void Update()
    {
         if (GlobalGems.karinCount == 10 || GlobalGems.karinCount == 30 || GlobalGems.karinCount == 50 || GlobalGems.karinCount == 100 || GlobalGems.karinCount == 150 || GlobalGems.karinCount == 200 || GlobalGems.karinCount == 250) // If the space bar is pushed down
         {
                ChangeTheDarnSprite(); // call method to change sprite
         }
         switch (GlobalGems.karinCount)
        {
            case 1:
                t6.SetActive(true);
                break;
            case 15:
                t1.SetActive(true);
                break;
            case 26:
                t4.SetActive(true);
                break;
            case 37:
                t2.SetActive(true);
                break;
            case 58:
                t11.SetActive(true);
                break;
            case 83:
                t9.SetActive(true);
                break;
            case 90:
                t3.SetActive(true);
                break;
            case 116:
                t12.SetActive(true);
                break;
            case 128:
                t13.SetActive(true);
                break;
            case 139:
                t5.SetActive(true);
                break;
            case 167:
                t8.SetActive(true);
                break;
            case 180:
                t7.SetActive(true);
                break;
            case 190:
                t10.SetActive(true);
                break;
            case 222:
                t6.SetActive(true);
                break;
        }
    }

    public void ChangeTheDarnSprite()
    {
       
        switch (GlobalGems.karinCount)
        {
            case 10:
                GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("KFC", "");
                Ruby.SetActive(true);
                break;
            case 30:
                GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("KFC", "");
                Saphire.SetActive(true);
                break;
            case 50:
                GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("KFC", "");
                Diamond.SetActive(true);
                break;
            case 100:
                GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("KFC", "");
                Good.SetActive(true);
                break;
            case 150:
                GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("KFC", "");
                Great.SetActive(true);
                break;
            case 200:
                Ultima.SetActive(true);
                break;
        }
           
    }
}
