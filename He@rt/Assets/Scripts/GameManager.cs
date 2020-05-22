using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    private PhotonView PV;

    public GameObject PlayMenu;

    public GameObject LoadIcon;
    public GameObject CombatResultWin;
    public GameObject CombatResultLose;
    public GameObject FleeMessage;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        PlayMenu.SetActive(true);
        LoadIcon.SetActive(false);
        CombatResultWin.SetActive(false);
        CombatResultLose.SetActive(false);
        FleeMessage.SetActive(false);

        GetComponent<TextManager>().OutputTextMain("");
    }

    public void SuccessfullFleeing()
    {
        PV.RPC("RPC_Flee", RpcTarget.All);
    }

    public void Return2Traversal(string result)
    {
        PV.RPC("RPC_LoadBack", RpcTarget.All, result);

        if (result == "Win" & TraversalManager.BossBattle)
        {
            PV.RPC("RPC_SetEndScreen", RpcTarget.All, true);
            StartCoroutine(LoadEndScreen());
        }
        else if (result == "Lose" && GetComponent<EntityCreation>().GetTeamHealthLevel() == 0)
        {
            PV.RPC("RPC_SetEndScreen", RpcTarget.All, true);
            StartCoroutine(LoadEndScreen());
        }
        else
        { StartCoroutine(LoadTraversal()); }
    }

    public IEnumerator LoadTraversal()
    {
        yield return new WaitForSeconds(GameObject.Find("SFX Manager").GetComponent<SFXManager>().GetClipLength() + 0.5f);
        SceneManager.LoadScene("Traversal");
    }

    public IEnumerator LoadEndScreen()
    {
        yield return new WaitForSeconds(GameObject.Find("SFX Manager").GetComponent<SFXManager>().GetClipLength() + 0.5f);
        SceneManager.LoadScene("EndScreen");
    }

    [PunRPC]
    private void RPC_LoadBack(string result)
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(result, "");

        if (result == "Win")
            CombatResultWin.SetActive(true);
        if (GetComponent<EntityCreation>().GetTeamHealthLevel() == 0)
            CombatResultLose.SetActive(true);

        LoadIcon.SetActive(true);

        StartCoroutine(GetComponent<AudioManager>().FadeOut(3f));
    }

    [PunRPC]
    private void RPC_Flee()
    {
        FleeMessage.SetActive(true);
        TraversalManager.JustFled = true;
        if (PhotonNetwork.IsMasterClient)
        { Return2Traversal("Lose"); }
    }

    [PunRPC]
    private void RPC_SetEndScreen(bool state)
    {
        EndScreenManager.BeatGame = state;
    }
}
