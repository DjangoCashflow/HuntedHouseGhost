using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ObjectController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject[] doors = new GameObject[15];

    void Start()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetActive(false); // Ensure the doors are disabled by default
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode >= 1 && eventCode <= 15) // Enable doors
        {
            doors[eventCode - 1].SetActive(true);
        }
        else if (eventCode >= 100 && eventCode <= 114) // Disable doors
        {
            doors[eventCode - 100].SetActive(false);
        }
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
