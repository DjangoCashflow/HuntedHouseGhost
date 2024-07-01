using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviourPunCallbacks
{
    public void OnRestartButtonClicked()
    {
        Debug.Log("Restart Button Clicked. Returning to GameScene.");
        // Disconnect from the current room before loading the scene
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the room. Loading GameScene.");
        PhotonNetwork.LoadLevel("GameScene");
    }
}
