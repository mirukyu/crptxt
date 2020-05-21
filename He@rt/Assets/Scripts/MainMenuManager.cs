using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public GameObject MainPageCanvas;
    public GameObject NetworkCanvas;
    public GameObject LoreCanvas;
    public GameObject TutorialCanvas;
    public GameObject RoomCanvas;

    public GameObject NetworkButtons;
    public GameObject RoomSelectionButtons;

    public GameObject StartButton;

    public InputField inputField;
    public InputField NetworkIDField;

    public Text TextField;

    public Text PlayerCount;
    public Text RoomName;
    public Text Players;

    public void Start()
    {
        MainPageCanvas.SetActive(true);
        NetworkCanvas.SetActive(false);
        LoreCanvas.SetActive(false);
        TutorialCanvas.SetActive(false);
        RoomCanvas.SetActive(false);
        StartButton.SetActive(false);
    }

    public bool VerifyInput(string input, string type)
    {
        if (input.Length == 0)
        {
            TextField.text = "Please enter the " + type + " you want to use.";
            return false;
        }

        if (input.Length > 16)
        {
            TextField.text = "The " + type + " is Too Long. Would you mind changing it?";
            return false;
        }
        return true;
    }

    public void ResetTextField()
    { TextField.text = "What Game Style do you want to play?"; }

    public void JoiningCustomFailed()
    { TextField.text = "申し訳ございませんでした。No such room was found.";  }

    public void RoomEnter(string PlayerCountText, string room, Photon.Realtime.Player[] players)
    {
        NetworkCanvas.SetActive(false);
        RoomCanvas.SetActive(true);

        PlayerCount.text = PlayerCountText;
        RoomName.text = "Room Name: " + room;
        Players.text = "";

        for (int i = 0; i < players.Length; i++)
        {
            Players.text += "\n" + players[i].NickName;
        }
    }

    public string GetNetworkName()
    { return NetworkIDField.text; }

    public void IsMaster(bool state)
    { StartButton.SetActive(state);  }

    //
    // All Below Are Only Button Functions
    //
    public void Play() // Main Page Canvas
    {
        MainPageCanvas.SetActive(false);
        NetworkCanvas.SetActive(true);

        RoomSelectionButtons.SetActive(false);
        NetworkButtons.SetActive(true);

        ResetTextField();

        GetComponent<AudioManager>().PlayTheme(ThemeStyle.Ambient);
    }

    public void PlaySolo() // Netwrok Canvas - Network Buttons
    {
        GetComponent<Matchmaking>().CreatSoloRoom();
        //GetComponent<RoomController>().SoloGame = true;
    }

    public void PlayOnline()
    {
        RoomSelectionButtons.SetActive(true);
        NetworkButtons.SetActive(false);

        TextField.text = "Choose your network functions. \n If you want to create a room or join a custom one, please fill out the input field with the wanted Room Name.";
    }

    public void PlayTurorial()
    {
        NetworkCanvas.SetActive(false);
        TutorialCanvas.SetActive(true);
    }

    public void Lore()
    {
        NetworkCanvas.SetActive(false);
        LoreCanvas.SetActive(true);
    }

    public void Quit()
    { Application.Quit(); }

    public void CreateRoom() // Netwrok Canvas - Room Selection Buttons
    {
        if (!VerifyInput(inputField.text, "Room Name") || !VerifyInput(NetworkIDField.text, "NetworkID"))
            return;

        GetComponent<Matchmaking>().CreateCustomRoom(inputField.text);
    }

    public void JoinCustomRoom()
    {
        if (!VerifyInput(inputField.text, "Room Name") || !VerifyInput(NetworkIDField.text, "NetworkID"))
            return;

        GetComponent<Matchmaking>().JoinCustomRoom(inputField.text);
    }

    public void JoinRandomRoom()
    {
        if (!VerifyInput(NetworkIDField.text, "NetworkID"))
            return;

        //PhotonNetwork.NickName = NetworkIDField.text;
        GetComponent<Matchmaking>().JoinRandomRoom();
    }

    public void StartGame() // Room Canvas
    { GetComponent<RoomController>().StartGame(); }

    public void Return() // Return
    {
        if (RoomCanvas.activeInHierarchy)
        { Matchmaking.LeaveRoom(); }

        RoomCanvas.SetActive(false);
        LoreCanvas.SetActive(false);
        TutorialCanvas.SetActive(false);
        NetworkCanvas.SetActive(true);
        RoomSelectionButtons.SetActive(false);
        NetworkButtons.SetActive(true);

        StartButton.SetActive(false);

        ResetTextField();
    }
}
