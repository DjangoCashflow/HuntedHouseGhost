using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ObjectController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject[] cubes = new GameObject[15];

    void Start()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].SetActive(false); // Ensure the cubes are disabled by default
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode >= 1 && eventCode <= 15) // Enable cubes
        {
            int index = eventCode - 1;
            cubes[index].SetActive(true);
            Debug.Log($"ObjectController: Cube {index} enabled");
        }
        else if (eventCode >= 100 && eventCode <= 114) // Disable cubes
        {
            int index = eventCode - 100;
            cubes[index].SetActive(false);
            Debug.Log($"ObjectController: Cube {index} disabled");
        }
    }

    public override void OnJoinedRoom()
    {
        // Initialize cube states when joining a room
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i].SetActive(false);
        }
        Debug.Log("ObjectController: Joined room, initialized cube states");
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}