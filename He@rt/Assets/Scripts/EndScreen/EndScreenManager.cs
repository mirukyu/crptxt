using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour {

    [SerializeField] private GameObject MainBackground;
    [SerializeField] private GameObject DevTeam;
    [SerializeField] private GameObject SeeYa;
    [SerializeField] private GameObject FunnyText1;
    [SerializeField] private GameObject FunnyText2;
    [SerializeField] private GameObject FunnyText3;
    [SerializeField] private GameObject FunnyText4;
    [SerializeField] private GameObject FunnyText5;
    [SerializeField] private GameObject ThankYouForPlaying;

    [SerializeField] private GameObject WinMessage;
    [SerializeField] private GameObject LoseMessage;

    public static bool BeatGame = false;

    private void Start()
    {
        PhotonNetwork.LeaveRoom();

        StartCoroutine(PlayAudio());

        if (BeatGame)
        { WinMessage.SetActive(true); LoseMessage.SetActive(false); }
        else
        { WinMessage.SetActive(false); LoseMessage.SetActive(true); }

        MainBackground.SetActive(true);
        StartCoroutine(FadeInColor(MainBackground, MainBackground.GetComponent<RawImage>().color, "GameObject", 1.5f));

        DevTeam.SetActive(true);
        StartCoroutine(FadeInColor(DevTeam, DevTeam.GetComponent<Text>().color, "Text", 10f));

        SeeYa.SetActive(true);
        StartCoroutine(FadeInColor(SeeYa, SeeYa.GetComponent<Text>().color, "Text", 42f));

        FunnyText1.SetActive(true);
        StartCoroutine(FadeInColor(FunnyText1, FunnyText1.GetComponent<Text>().color, "Text", 69f));

        FunnyText2.SetActive(true);
        StartCoroutine(FadeInColor(FunnyText2, FunnyText2.GetComponent<Text>().color, "Text", 77f));

        FunnyText3.SetActive(true);
        StartCoroutine(FadeInColor(FunnyText3, FunnyText3.GetComponent<Text>().color, "Text", 88f));

        FunnyText4.SetActive(true);
        StartCoroutine(FadeInColor(FunnyText4, FunnyText4.GetComponent<Text>().color, "Text", 103f));

        FunnyText5.SetActive(true);
        StartCoroutine(FadeInColor(FunnyText5, FunnyText5.GetComponent<Text>().color, "Text", 115f));

        ThankYouForPlaying.SetActive(true);
        StartCoroutine(FadeInColor(ThankYouForPlaying, ThankYouForPlaying.GetComponent<Text>().color, "Text", 137f));
    }

    private IEnumerator FadeInColor(GameObject Reference, Color myColor, string type, float delay)
    {
        float alpha = 0f;

        if (type == "GameObject")
            Reference.GetComponent<RawImage>().color = new Color(myColor.r, myColor.g, myColor.b, alpha);
        else
            Reference.GetComponent<Text>().color = new Color(myColor.r, myColor.g, myColor.b, alpha);

        float currentTime = 0f;
        float FadeDuration = 5f;
        yield return new WaitForSeconds(delay);

        while (currentTime < FadeDuration)
        {
            currentTime += Time.deltaTime;
            alpha = Mathf.Lerp(0f, 1f, currentTime / FadeDuration);

            if (type == "GameObject")
                Reference.GetComponent<RawImage>().color = new Color(myColor.r, myColor.g, myColor.b, alpha);
            else
                Reference.GetComponent<Text>().color = new Color(myColor.r, myColor.g, myColor.b, alpha);
            yield return null;
        }
        yield break;
    }

    public IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<AudioManager>().PlayTheme(ThemeStyle.End);

        StartCoroutine(QuitAfterAudio());
    }

    private IEnumerator QuitAfterAudio()
    {
        yield return new WaitForSeconds(GetComponent<AudioManager>().GetClipLength() + 0.5f);
        Quit();
    }

    public void Quit()
    { Application.Quit(); }
}
