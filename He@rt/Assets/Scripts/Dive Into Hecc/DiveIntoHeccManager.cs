using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiveIntoHeccManager : MonoBehaviour
{

    public static List<int> Results;

    public Text TimeText;
    public Text RewardManaText;
    public Text RewardHealthText;
    private float maxTime = 60f;
    private float time = 0f;
    private int rewardMana = 0;
    private int rewardHealth = 0;

    private int MaxHealthReward = 25;
    private int MaxManaReward = 50;

    public Text DeathText;

    private int myID;
    private PhotonView PV;
    private int PlayersFinished = 0;

    public GameObject WaitScreen;
    public GameObject LoadBackScreen;
    public GameObject LoadIcon;

    public static bool StopPlaying;

    public void Start()
    {
        Results = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };

        WaitScreen.SetActive(false);
        LoadBackScreen.SetActive(false);
        LoadIcon.SetActive(false);
        DeathText.text = "";

        myID = CharacterSetUp.Game.myID;
        PV = GetComponent<PhotonView>();

        StopPlaying = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(PlayAudio());
    }

    // Update is called once per frame
    void Update()
    {
        if (!StopPlaying)
        {
            time += Time.deltaTime;
            TimeText.text = "Time: " + System.String.Format("{0:0.00}", time);
            rewardMana = Mathf.RoundToInt(Mathf.Lerp(0, MaxManaReward, time / maxTime));
            rewardHealth = Mathf.RoundToInt(Mathf.Lerp(0, MaxHealthReward, time / maxTime));
            RewardManaText.text = "Mana Reward: " + rewardMana.ToString();
            RewardHealthText.text = "Health Reward: " + rewardHealth.ToString();
        }
    }

    public IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<AudioManager>().PlayTheme(ThemeStyle.MinigameDiveIntoHecc);
        StartCoroutine(GetComponent<AudioManager>().FadeIn(1f));
        yield return new WaitForSeconds(GetComponent<AudioManager>().GetClipLength());
        StartCoroutine(PlayAudio());
    }

    public IEnumerator Died()
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("DiveIntoHeck", "");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StopPlaying = true;
        WaitScreen.SetActive(true);
        DeathText.text = "You Died!\nYou have earned\n" + rewardHealth + " Health points &\n " + rewardMana + " Mana points.";
        yield return new WaitForSeconds(GameObject.Find("SFX Manager").GetComponent<SFXManager>().GetClipLength() + 0.1f);
        PV.RPC("RPC_SetResult", RpcTarget.MasterClient, myID, rewardHealth, rewardMana);
    }

    public void Return2Traversal()
    {
        PV.RPC("RPC_LoadingBack", RpcTarget.All);
        StartCoroutine(LoadTraversal());
        StartCoroutine(GetComponent<AudioManager>().FadeOut(3f));
    }

    public IEnumerator LoadTraversal()
    {
        yield return new WaitForSeconds(GameObject.Find("SFX Manager").GetComponent<SFXManager>().GetClipLength() + 1f);
        SceneManager.LoadScene("Traversal");
    }

    [PunRPC]
    private void RPC_SetResult(int ID, int Health, int Mana)
    {
        PlayersFinished += 1;
        Results[2 * ID] = Health;
        Results[2 * ID + 1] = Mana;

        if (PlayersFinished >= RoomController.room.playersInRoom)
        { Return2Traversal(); }
    }

    [PunRPC]
    private void RPC_LoadingBack()
    {
        WaitScreen.SetActive(false);
        LoadBackScreen.SetActive(true);
        LoadIcon.SetActive(true);
        Time.timeScale = 1f;
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("MinigameEnd", "");
    }
}
