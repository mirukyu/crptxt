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
    public GameObject PauseMenu;
    private bool escape_state;
    private bool just_changed_escape_state;

    public GameObject LoadIcon;
    public GameObject CombatResultWin;
    public GameObject CombatResultLose;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        PauseMenu.SetActive(false);
        PlayMenu.SetActive(true);
        LoadIcon.SetActive(false);
        CombatResultWin.SetActive(false);
        CombatResultLose.SetActive(false);

        string OutputTextTest = "This is purely an example as I have zero idea as what this should do. Hipefully it works as expected because I dont want to waste time.";
        GetComponent<TextManager>().OutputTextMain(OutputTextTest);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Pause
        {
            if (escape_state == false && just_changed_escape_state == false)
            {
                /*
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                */
                Time.timeScale = 0;
                escape_state = true;
                PlayMenu.SetActive(false);
                PauseMenu.SetActive(true);
                just_changed_escape_state = true;
            }
            if (escape_state == true && just_changed_escape_state == false)
            {
                /*
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                */
                Time.timeScale = 1;
                escape_state = false;
                PlayMenu.SetActive(true);
                PauseMenu.SetActive(false);
            }
            just_changed_escape_state = false;
        }
    }

    public void PressButton(string ButtonFunction) //Buttons in Pause Menu
    {
        switch (ButtonFunction)
        {
            case "Restart":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "ExitGame":
                Application.Quit();
                break;
            case "BackToMainMenu":
                SceneManager.LoadScene("Main Menu");
                break;
            case "Resume":
                escape_state = false;
                PlayMenu.SetActive(true);
                PauseMenu.SetActive(false);
                break;
        }
        Time.timeScale = 1;
        /*
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        */
    }

    public void Return2Traversal(string result)
    {
        PV.RPC("RPC_LoadBack", RpcTarget.All, result);
        
        StartCoroutine(LoadTraversal());
    }

    public IEnumerator LoadTraversal()
    {
        yield return new WaitForSeconds(GameObject.Find("SFX Manager").GetComponent<SFXManager>().GetClipLength() + 0.5f);
        SceneManager.LoadScene("TMPTraversal");
    }

    [PunRPC]
    private void RPC_LoadBack(string result)
    {
        GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX(result, "");

        if (result == "Win")
            CombatResultWin.SetActive(true);
        else
            CombatResultLose.SetActive(true);

        LoadIcon.SetActive(true);

        StartCoroutine(GetComponent<AudioManager>().FadeOut(3f));
    }
}
