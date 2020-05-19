using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CharacterMenuManager : MonoBehaviourPunCallbacks
{
    public static PhotonView PV;

    public GameObject CharacterSelectCanvas;
    public InputField inputField;

    public Text TextField;
    public GameObject LoadingIcon;

    public GameObject Barrier;
    public GameObject ConfirmButton;

    [SerializeField] private GameObject Player1SpawnPoint;
    [SerializeField] private GameObject Player2SpawnPoint;
    [SerializeField] private GameObject Player3SpawnPoint;
    [SerializeField] private GameObject Player4SpawnPoint;

    public GameObject CharacterField1;
    public GameObject CharacterField2;
    public GameObject CharacterField3;
    public GameObject CharacterField4;

    public static string player1Name;
    public static EntityType player1;
    public static string player2Name;
    public static EntityType player2;
    public static string player3Name;
    public static EntityType player3;
    public static string player4Name;
    public static EntityType player4;

    private GameObject PreviousCharacter;
    private EntityType tmpCharacter = EntityType.DEFAULT;
    private int counter = 1;

    private bool started = false;

    public void Start()
    {
        PV = GetComponent<PhotonView>();

        CharacterSelectCanvas.SetActive(true);
  
        LoadingIcon.SetActive(false);

        Barrier.SetActive(false);
        ConfirmButton.SetActive(true);

        UpdateSelectionColour();

        StartCoroutine(PlayAudio());
    }

    public void Update()
    {
        if (!started && counter > RoomController.room.playersInRoom)
        {
            started = true;

            LoadingIcon.SetActive(true);
            TextField.text = "Loading Game ...";

            GameObject.Find("Character Setup").GetComponent<CharacterSetUp>().SetUp();

            StartCoroutine(LoadNextScene());
        }
    }

    public IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<AudioManager>().PlayTheme(ThemeStyle.Unique);
    }

    public IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("TMPTraversal");
    }

    public void PressButton(string Function)
    {
        switch (Function)
        {
            case "OK":
                if (inputField.text == "Wisely" || inputField.text == "Changed Later" || inputField.text == "Your Nickname") // Jokes depending on the name input
                {
                    switch (inputField.text)
                    {
                        case "Wisely":
                            TextField.text = "Oh I see. We have a real funny one here. Ya think I'll accept your BS?";
                            break;
                        case "Changed Later":
                            TextField.text = "Now listen here you little f...";
                            break;
                        case "Your Nickname":
                            TextField.text = "How about using this one instead?";
                            inputField.text = "失望";
                            break;
                    }
                    return;
                }

                switch (inputField.text) // Predefines Names / To Get Access to Asian Characters -- Jokes Only
                {
                    case "Alex":
                        inputField.text = "アレックス";
                        break;
                    case "Yahiko":
                        inputField.text = "やひこ";
                        break;
                    case "Shachou":
                        inputField.text = "社長";
                        break;
                    case "Kim Hyeon U":
                        inputField.text = "김현우";
                        break;

                    case "Grace":
                        inputField.text = "하얀 백조";
                        break;
                    case "Jiyoung":
                        inputField.text = "지영아";
                        break;

                    case "Karim":
                        inputField.text = "Karim-ちゃん";
                        break;

                    case "Motaz":
                        inputField.text = "Motta :)";
                        break;
                }

                if (inputField.text != "" && inputField.text.Length <= 16 && tmpCharacter != EntityType.DEFAULT) // actual important part
                {
                    //TextField.text = "Please choose your nickname and character. Choose wisely as it can't be changed later."; // Basis
                    //PreviousCharacter.GetComponent<CharacterSelectionManager>().Selecting();
                    GameObject.Find("Character Setup").GetComponent<CharacterSetUp>().SetMyID(counter - 1);   

                    PV.RPC("RPC_ShareSelect", RpcTarget.All, PreviousCharacter.GetComponent<CharacterSelectionManager>().ListID, null, "Confirm");
                    
                    PV.RPC("RPC_ConfirmingCharacter", RpcTarget.All, inputField.text, tmpCharacter);

                    Barrier.SetActive(true);
                    ConfirmButton.SetActive(false);

                    inputField.text = "";
                    TextField.text = "Waiting for other Players";  

                    tmpCharacter = EntityType.DEFAULT;
                }
                else
                {
                    if (inputField.text == "")
                    { TextField.text = "Please enter your nickname."; }
                    if (inputField.text.Length > 16)
                    { TextField.text = "The chosen nickname is too long."; }
                    if (tmpCharacter == EntityType.DEFAULT)
                    { TextField.text = "Please choose your character."; }
                }

                //UpdateSelectionColour();
                break;
        }
    }

    [PunRPC]
    private void RPC_ConfirmingCharacter(string NameToUse, EntityType Character)
    {
        GameObject PlaceToUse = null;
        string ModelToUse = null;

        switch (counter) // Taking the respective variables for the corresponding player
        {
            case 1:
                player1Name = NameToUse;
                player1 = Character;
               
                PlaceToUse = Player1SpawnPoint;
                ModelToUse = GetModel(player1);
                break;
            case 2:
                player2Name = NameToUse;
                player2 = Character;

                PlaceToUse = Player2SpawnPoint;
                ModelToUse = GetModel(player2);
                break;
            case 3:
                player3Name = NameToUse;
                player3 = Character;

                PlaceToUse = Player3SpawnPoint;
                ModelToUse = GetModel(player3);
                break;
            case 4:
                player4Name = NameToUse;
                player4 = Character;

                PlaceToUse = Player4SpawnPoint;
                ModelToUse = GetModel(player4);
                break;
        }

        Instantiate(Resources.Load<GameObject>(ModelToUse), PlaceToUse.transform.position, PlaceToUse.transform.rotation); // Instantiation
        Object tmp = Instantiate(Resources.Load("Floating Name"), PlaceToUse.transform.position + new Vector3(0, 1.5f, 0), PlaceToUse.transform.rotation);
        ((GameObject)tmp).GetComponent<TextMesh>().text = NameToUse;

        counter++;
    }

    public void GetCharacter(GameObject CharacterField) // receives the input from the button and resets the previous selected field / deselcts the current one if pressed again
    {
        if (CharacterField == PreviousCharacter) // Deselecting
        {
            CharacterField.GetComponent<CharacterSelectionManager>().Deselect();
            PV.RPC("RPC_ShareSelect", RpcTarget.Others, CharacterField.GetComponent<CharacterSelectionManager>().ListID, false, "Select");
            PreviousCharacter = null;
            tmpCharacter = EntityType.DEFAULT;
        }
        else // Selecting
        {
            if (PreviousCharacter)
            {
                PreviousCharacter.GetComponent<CharacterSelectionManager>().Deselect();
                PV.RPC("RPC_ShareSelect", RpcTarget.Others, PreviousCharacter.GetComponent<CharacterSelectionManager>().ListID, false, "Select");
            }

            CharacterField.GetComponent<CharacterSelectionManager>().Select();
            PV.RPC("RPC_ShareSelect", RpcTarget.Others, CharacterField.GetComponent<CharacterSelectionManager>().ListID, true, "Select");
            tmpCharacter = CharacterField.GetComponent<CharacterSelectionManager>().GetCharacter();

            PreviousCharacter = CharacterField;
        }

        UpdateSelectionColour();
    }

    
    [PunRPC]
    private void RPC_ShareSelect(int i, bool state, string type) //
    {
        GameObject[] tmp = { CharacterField1, CharacterField2, CharacterField3, CharacterField4 };

        if (type == "Select")
            tmp[i].GetComponent<CharacterSelectionManager>().SelectedByOtherPlayer = state;
        if (type == "Confirm")
            tmp[i].GetComponent<CharacterSelectionManager>().Selecting();
        //tmp[i].GetComponent<CharacterSelectionManager>().ApplySelectionColour();
        UpdateSelectionColour();
    }

    public void UpdateSelectionColour() // changes colour of all selection fields
    {
        CharacterField1.GetComponent<CharacterSelectionManager>().ApplySelectionColour();
        CharacterField2.GetComponent<CharacterSelectionManager>().ApplySelectionColour();
        CharacterField3.GetComponent<CharacterSelectionManager>().ApplySelectionColour();
        CharacterField4.GetComponent<CharacterSelectionManager>().ApplySelectionColour();
    }

    public static string GetModel(EntityType character) // links the entity type to the player model
    {
        switch (character)
        {
            case EntityType.Aramusha:
                return "Models/Persona/Aramusha";
            case EntityType.Priestress:
                return "Models/Persona/Priestess";
            case EntityType.Mage:
                return "Models/Persona/Mage";
            case EntityType.Crusader:
                return "Models/Persona/Crusader";
        }

        return "";
    }
}
