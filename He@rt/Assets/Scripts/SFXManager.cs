using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // This is destined to play all the SFX sounds
    // this should be placed on a seperate object with its own AudioSource
    // (the reason being that we need two different audiosources for themes and SFX)

    // Loading Type of these files may be DECOMPRESS ON LOAD

    // GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX();
    // Use this for initialization

    private AudioSource myAudio;

    private Object[] Win;
    private Object[] Lose;
    private Object[] Footsteps;
    private Object[] MinigameEnd;
    private Object[] DiveIntoHeck;
    private Object[] KFC;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        
        Win = Resources.LoadAll("SFX/Win", typeof(AudioClip));
        Lose = Resources.LoadAll("SFX/Lose", typeof(AudioClip));
        Footsteps = Resources.LoadAll("SFX/Footsteps", typeof(AudioClip));
        MinigameEnd = Resources.LoadAll("SFX/Miscellaneous", typeof(AudioClip));
        DiveIntoHeck = Resources.LoadAll("SFX/DiveIntoHeck", typeof(AudioClip));
        KFC = Resources.LoadAll("SFX/KFC", typeof(AudioClip));
    }

    public void PlaySFX(string sound_name, string FolderName)
    {
        myAudio.clip = null;
        myAudio.volume = PauseMenuManager.SFXVolume;

        switch (sound_name)
        {
            case "Win":
                GameObject.Find("Game Manager Battle").GetComponent<AudioManager>().StopPlay();
                myAudio.clip = (AudioClip)Win[Random.Range(0, Win.Length)];
                break;
            case "Lose":
                GameObject.Find("Game Manager Battle").GetComponent<AudioManager>().StopPlay();
                myAudio.clip = (AudioClip)Lose[Random.Range(0, Lose.Length)];
                StartCoroutine(GameObject.Find("Game Manager Battle").GetComponent<AudioManager>().UnpausePlay(myAudio.clip.length));
                break;
            case "Footsteps":
                myAudio.clip = (AudioClip)Footsteps[Random.Range(0, Footsteps.Length)];
                break;
            case "MinigameEnd":
                myAudio.clip = (AudioClip)MinigameEnd[Random.Range(0, MinigameEnd.Length)];
                break;
            case "DiveIntoHeck":
                myAudio.clip = (AudioClip)DiveIntoHeck[Random.Range(0, DiveIntoHeck.Length)];
                break;
            case "KFC":
                myAudio.clip = (AudioClip)KFC[Random.Range(0, KFC.Length)];
                break;
        }

        if (myAudio.clip == null)
        { myAudio.clip = Resources.Load<AudioClip>("SFX/" + FolderName + "/" + sound_name); }

        if (myAudio.clip == null)
        { myAudio.clip = Resources.Load<AudioClip>("Ara Ara"); }

        myAudio.Play();
    }

    public float GetClipLength()
    {
        if (!myAudio.isPlaying)
            return 0;
        return myAudio.clip.length;
    }
}
