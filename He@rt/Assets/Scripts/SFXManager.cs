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

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        
        Win = Resources.LoadAll("SFX/Win", typeof(AudioClip));
        Lose = Resources.LoadAll("SFX/Lose", typeof(AudioClip));
    }

    public void PlaySFX(string sound_name, string FolderName)
    {
        myAudio.clip = null;
        myAudio.volume = 0.5f;

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
        }

        if (myAudio.clip == null)
        { myAudio.clip = Resources.Load<AudioClip>("SFX/" + FolderName + "/" + sound_name); }

        if (myAudio.clip == null)
        { myAudio.clip = Resources.Load<AudioClip>("Ara Ara"); }

        myAudio.Play();
    }

    public float GetClipLength()
    { return myAudio.clip.length; }
}
