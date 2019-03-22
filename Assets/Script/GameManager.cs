using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour
{
    private int playerCount = 0;

    private void Start()
    {
        PhotonNetwork.Instantiate("Model_Player", new Vector3(Random.RandomRange(0,10.0f), 2, Random.RandomRange(0, 10.0f)), Quaternion.identity, 0);
    }

}
