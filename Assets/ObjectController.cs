using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ObjectController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject cube;

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == 1) // Enable cube
        {
            cube.SetActive(true);
        }
        else if (eventCode == 0) // Disable cube
        {
            cube.SetActive(false);
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
