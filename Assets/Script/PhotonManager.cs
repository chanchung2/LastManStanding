using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : Photon.PunBehaviour
{

    [SerializeField] InputField inputRoomName;
    [SerializeField] InputField inputID;
    [SerializeField] GameObject roomLayout;
    [SerializeField] GameObject roomPrefab;

    [SerializeField] GameObject roomUI;
    [SerializeField] GameObject joinLobbyUI;

    [SerializeField] GameObject userLayout;
    [SerializeField] GameObject userPrefab;
    [SerializeField] GameObject startButton;
    [SerializeField] InputField inputNPC;

    [SerializeField] GameObject npcUI;

    private bool isReady = false;

    public static PhotonManager instance;

    private void Awake()
    {
        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 20;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings("1.0");

        if (instance != null)
        {
            DestroyImmediate(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        //InvokeRepeating("UpdatePing", 2, 2);
    }

    void UpdatePing()
    {
        int pingRate = PhotonNetwork.GetPing();
        Debug.Log("Ping : " + pingRate);
    }

    private void OnJoinedLobby()
    {
        //Debug.Log("Entered Lobby");
    }

    private void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("Player Connected");

        UserListUpdate();
    }
    private void OnPhotonPlayerDisconnected(PhotonPlayer disPlayer)
    {
        Debug.Log("Player Disconnected");

        UserListUpdate();
    }

    private void OnJoinedRoom()
    {
        //Debug.Log("Enter Room");

        roomUI.SetActive(false);
        joinLobbyUI.SetActive(true);

        UserListUpdate();

        //StartCoroutine(LoadMain());
    }

    private void OnPhotonCreateRoomFailed(object[] error)
    {
        Debug.Log(error[0].ToString());
        Debug.Log(error[1].ToString());
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public void OnClickJoinRoom(string _roomName)
    {
        PhotonNetwork.player.name = inputID.text;

        startButton.gameObject.SetActive(false);
        npcUI.SetActive(false);
        PhotonNetwork.JoinRoom(_roomName);
    }

    public void OnClickCreateRoom()
    {
        string roomName = inputRoomName.text;

        PhotonNetwork.player.name = inputID.text;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.maxPlayers = 10;

        startButton.gameObject.SetActive(true);
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    private void OnReceivedRoomListUpdate()
    {
        if (roomLayout.transform.childCount >= 1)
        {
            foreach (RectTransform clone in roomLayout.GetComponentsInChildren<RectTransform>())
            {
                if (clone.transform.name != "RoomLayoutGroup")
                {
                    Destroy(clone.gameObject);
                }
            }
        }

        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            GameObject roomUI = Instantiate(roomPrefab);

            roomUI.transform.SetParent(roomLayout.transform, false);
            roomUI.transform.Find("RoomNameText").GetComponent<Text>().text = room.name;
            roomUI.transform.GetComponent<Button>().onClick.AddListener(delegate { OnClickJoinRoom(room.name); });
        }
    }

    public void OnClickExitButton()
    {
        PhotonNetwork.LeaveRoom();

        UserListUpdate();

        joinLobbyUI.SetActive(false);
        roomUI.SetActive(true);
    }

    public void OnClickStartButton()
    {
        RoomMakeInfo.instance.playerCount = PhotonNetwork.countOfPlayers;
        RoomMakeInfo.instance.npcCount = int.Parse(inputNPC.text);

        PhotonNetwork.LoadLevel("Main");
    }

    public void UserListUpdate()
    {
        foreach (RectTransform clone in userLayout.GetComponentsInChildren<RectTransform>())
        {
            if (clone.transform.name != "UserLayoutGroup")
            {
                Destroy(clone.gameObject);
            }
        }

        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            GameObject userUI = Instantiate(userPrefab);

            userUI.transform.SetParent(userLayout.transform, false);
            userUI.transform.Find("UserNameText").GetComponent<Text>().text = "ID : " + player.name;
        }
    }
}
