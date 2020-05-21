using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThemeStyle
{ Ambient, Battle, Boss, End, EventMystic, EventNegative, EventPositive, MinigameKFC, MinigameDiveIntoHecc, Suspense, TraversalCastle , TraversalCave, TraversalFields, TraversalTown, Traversal, Unique};

public class AudioManager : MonoBehaviour {

    // This is destined to play all the Theme / Ambient / etc. sounds
    // this should be placed on the GameManager with its own AudioSource
    // (the reason being that we need two different audiosources for themes and SFX)

    // PROBLEM: LOADING IN THE FILES THIS WAY TAKES TIME
    // IMPORTANT NOTICE: TO SOLVE THAT PROBLEM SET LOADING TYPE OF FILES TO STREAMING

    // GameObject.Find("Game Manager").GetComponent<AudioManager>().PlayRandom();
    // Use this for initialization

    private AudioSource myAudio;

    private Object[] Playlist;

    public void Start()
    { myAudio = GetComponent<AudioSource>(); }

    public void PlayRandom() // Plays a random Audio File
    {
        ThemeStyle tmp = (ThemeStyle)Random.Range(0, System.Enum.GetValues(typeof(ThemeStyle)).Length);
        PlayTheme(tmp);
    }

    public float GetClipLength()
    { return myAudio.clip.length; }

    public void StopPlay() // Pause and Unpause Functions
    { myAudio.Pause(); }
    public IEnumerator UnpausePlay(float delay) 
    {
        yield return new WaitForSeconds(delay);                                   // UNPAUSE PLAY and FADEIN and FADEOUT have to be called with StartCoroutine();;
        myAudio.Play();
        StartCoroutine(FadeIn(3f));
    }

    public IEnumerator FadeIn(float duration)  // Fade In
    { StartCoroutine(FadeEffect(duration, 0f, myAudio.volume)); yield return null; }
    public IEnumerator FadeOut(float duration) // Fade Out
    { StartCoroutine(FadeEffect(duration, myAudio.volume, 0f)); yield return null; }
    public IEnumerator FadeEffect(float duration, float startVolume, float targetVolume) // Fade Effect Basis
    {
        float currentTime = 0f;
        
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            myAudio.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }


    public void PlayTheme(ThemeStyle theme) // Calling the respective theme to play, from there takes a random file
    {
        myAudio.volume = PauseMenuManager.AudioVolume;
        string path = "";

        switch (theme)
        {
            case ThemeStyle.Ambient:
                path = "Ambient";
                break;
            case ThemeStyle.Battle:
                myAudio.volume *= 0.8f;
                path = "Battle";
                break;
            case ThemeStyle.Boss:
                myAudio.volume *= 0.7f;
                path = "Boss";
                break;
            case ThemeStyle.End:
                path = "End";
                break;
            case ThemeStyle.EventMystic:
                path = "Events/Mystic";
                break;
            case ThemeStyle.EventNegative:
                path = "Events/Negative";
                break;
            case ThemeStyle.EventPositive:
                path = "Events/Positive";
                break;
            case ThemeStyle.MinigameKFC:
                path = "MinigameKFC";
                break;
            case ThemeStyle.MinigameDiveIntoHecc:
                path = "MinigameDiveIntoHeck";
                break;
            case ThemeStyle.Suspense:
                path = "Suspense";
                break;
            case ThemeStyle.TraversalCastle:
                path = "Traversal/Castle";
                break;
            case ThemeStyle.TraversalCave:
                path = "Traversal/Cave";
                break;
            case ThemeStyle.TraversalFields:
                path = "Traversal/Fields";
                break;
            case ThemeStyle.TraversalTown:
                path = "Traversal/Town";
                break;
            case ThemeStyle.Traversal:
                List<ThemeStyle> TraversalList = new List<ThemeStyle> { ThemeStyle.TraversalCastle, ThemeStyle.TraversalCave, ThemeStyle.TraversalFields, ThemeStyle.TraversalTown };
                PlayTheme(TraversalList[Random.Range(0,TraversalList.Count)]);
                return;
            case ThemeStyle.Unique:
                path = "Unique";
                break;
        }

        Playlist = Resources.LoadAll("Audio/" + path, typeof(AudioClip));
        myAudio.clip = (AudioClip)Playlist[Random.Range(0, Playlist.Length)];

        myAudio.Play();
    }
}
