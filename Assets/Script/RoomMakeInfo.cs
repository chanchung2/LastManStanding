using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMakeInfo : MonoBehaviour
{
    public static RoomMakeInfo instance;

    public int npcCount;
    public int playerCount;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
}
