using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {

    [SerializeField] private GameObject PauseMenu;
    private bool escape_state;
    private bool just_changed_escape_state;

    [SerializeField] private Slider AudioSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Text AudioText;
    [SerializeField] private Text SFXText;

    public static float AudioVolume = 0.5f;
    public static float SFXVolume = 0.5f;

    private AudioSource myAudio;

    // Use this for initialization
    void Start () {
        PauseMenu.SetActive(false);
        AudioSlider.GetComponent<Slider>().value = AudioVolume;
        SFXSlider.GetComponent<Slider>().value = SFXVolume;

        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Pause
        {
            myAudio.clip = null;
            myAudio.volume = SFXVolume;

            if (escape_state == false && just_changed_escape_state == false)
            {
                escape_state = true;
                PauseMenu.SetActive(true);
                just_changed_escape_state = true;
                myAudio.clip = Resources.Load<AudioClip>("SFX/PauseMenu/Inventory_Open_00");
                myAudio.Play();
            }
            if (escape_state == true && just_changed_escape_state == false)
            {
                Resume();
            }
            just_changed_escape_state = false;
        }
    }

   public void Resume()
    {
        escape_state = false;
        PauseMenu.SetActive(false);
        myAudio.clip = Resources.Load<AudioClip>("SFX/PauseMenu/Inventory_Open_01");
        myAudio.Play();
    }

    public void ChangeAudio()
    {
        AudioVolume = AudioSlider.GetComponent<Slider>().value;
        AudioText.text = AudioVolume.ToString();
    }

    public void ChangeSFX()
    {
        SFXVolume = SFXSlider.GetComponent<Slider>().value;
        SFXText.text = SFXVolume.ToString();
    }
}
