using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KFCManager : MonoBehaviour {

    public static List<int> Results;

    public Text TimeText;
    public Text RewardManaText;
    public Text RewardHealthText;
    private float time = 37.5f;
    private bool finished = false;
    private int rewardMana = 0;
    private int rewardHealth = 0;

    private int MaxHealthReward = 40;
    private int MaxManaReward = 35;

    public Text DeathText;

    private int myID;
    private PhotonView PV;
    private int PlayersFinished = 0;

    public GameObject WaitScreen;
    public GameObject LoadBackScreen;
    public GameObject LoadIcon;

    public void Start()
    {
        Results = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };

        WaitScreen.SetActive(false);
        LoadBackScreen.SetActive(false);
        LoadIcon.SetActive(false);
        DeathText.text = "";

        myID = CharacterSetUp.Game.myID;
        PV = GetComponent<PhotonView>();

        StartCoroutine(PlayAudio());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!finished)
        {
            time -= Time.deltaTime;
            TimeText.text = "Time Left: " + System.String.Format("{0:0.00}", time) + " ";
            rewardMana = Mathf.RoundToInt(Mathf.Lerp(0, MaxManaReward, GlobalGems.karinCount / 200f));
            rewardHealth = Mathf.RoundToInt(Mathf.Lerp(0, MaxHealthReward, GlobalGems.karinCount / 200f));
            RewardManaText.text = "Mana Reward: " + rewardMana.ToString();
            RewardHealthText.text = "Health Reward: " + rewardHealth.ToString();

            if (time <= 0f || GlobalGems.karinCount >= 200)
            {
                finished = true;
                StartCoroutine(Died());
            }
        }
    }

    public IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<AudioManager>().PlayTheme(ThemeStyle.MinigameKFC);
        StartCoroutine(GetComponent<AudioManager>().FadeIn(1f));
        yield return new WaitForSeconds(GetComponent<AudioManager>().GetClipLength());
        StartCoroutine(PlayAudio());
    }

    public IEnumerator Died()
    {
        if (time <= 0f)
        { DeathText.text = "Time is over!"; }
        else
        { DeathText.text = "You finished in time! Congrats!"; }

        DeathText.text += "\nYou have earned\n" + rewardHealth + " Health points &\n " + rewardMana + " Mana points.";

        WaitScreen.SetActive(true);

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
        SceneManager.LoadScene("TMPTraversal");
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
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("MinigameEnd", "");
    }
}
