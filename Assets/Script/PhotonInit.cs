using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInit : Photon.PunBehaviour
{
    private void Awake()
    {
        PhotonNetwork.sendRate = 30;
        PhotonNetwork.sendRateOnSerialize = 20;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings("1.0");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnPhotonRandomJoinFailed()
    {
        Debug.Log("no room");

        PhotonNetwork.CreateRoom("My Room");
    }

    private void OnPhotonCreateRoomFailed(object[] error)
    {
        Debug.Log(error[0].ToString());
        Debug.Log(error[1].ToString());
    }

    private void OnJoinedRoom()  // 룸 입장시 호출되는 콜백 함수
    {
        Debug.Log("Enter Room");
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate("Model_Player", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
