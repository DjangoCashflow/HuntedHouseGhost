using Photon.Pun;
using TMPro;
using UnityEngine;

public class AndroidConnectionManager : MonoBehaviourPunCallbacks
{
    public TMP_Text connectionStatusText;

    void Start()
    {
        connectionStatusText.text = "Connecting...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        connectionStatusText.text = "Connected to Master";
        PhotonNetwork.JoinOrCreateRoom("ControlRoom", new Photon.Realtime.RoomOptions { MaxPlayers = 10 }, null);
    }

    public override void OnJoinedRoom()
    {
        connectionStatusText.text = "Joined room successfully.";
        Debug.Log("Joined room successfully.");
        SpawnPlayer();
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        connectionStatusText.text = $"Disconnected: {cause}";
        Debug.LogError($"Disconnected: {cause}");
    }

    void SpawnPlayer()
    {
        PhotonNetwork.Instantiate("AndroidPlayer", Vector3.zero, Quaternion.identity);
    }
}
