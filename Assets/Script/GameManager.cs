using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour
{
    private int playerCount = 0;

    [SerializeField] private GameObject spawnPos;

    private void Start()
    {
        PhotonNetwork.Instantiate("Model_Player", new Vector3(Random.RandomRange(0,10.0f), 2, Random.RandomRange(0, 10.0f)), Quaternion.identity, 0);

        if (PhotonNetwork.isMasterClient)
        {
            //Reset();
        }
    }

    private void Reset()
    {
        if (PhotonNetwork.isMasterClient == true)
        {
            for (int i = 0; i < RoomMakeInfo.instance.npcCount; i++)
            {
                while (true)
                {
                    int idx = Random.Range(0, spawnPos.transform.childCount + 1);

                    if (spawnPos.transform.GetChild(idx).gameObject.GetActive())
                    {
                        continue;
                    }
                    else
                    {
                        spawnPos.transform.GetChild(idx).gameObject.SetActive(true);
                        PhotonNetwork.Instantiate("Model_AI", spawnPos.transform.GetChild(idx).transform.position , Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0) ,0);
                        break;
                    }
                }
            }
        }
    }
}
