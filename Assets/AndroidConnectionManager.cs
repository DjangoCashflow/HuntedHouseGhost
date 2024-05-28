using Photon.Pun;
using TMPro;
using UnityEngine;

public class AndroidConnectionManager : MonoBehaviourPunCallbacks
{
    public TMP_Text connectionStatusText;

    void Start()
    {
        connectionStatusText.text = "Connecting to server...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        connectionStatusText.text = "Connected to Master";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        connectionStatusText.text = "Joined Lobby";
        PhotonNetwork.JoinOrCreateRoom("Room1", new Photon.Realtime.RoomOptions(), Photon.Realtime.TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        connectionStatusText.text = "Joined Room";
        PhotonNetwork.Instantiate("PlayerPrefab", Vector3.zero, Quaternion.identity);
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        connectionStatusText.text = $"Disconnected: {cause}";
    }
}
