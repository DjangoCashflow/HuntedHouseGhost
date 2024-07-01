using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Detected");

        if ((gameObject.CompareTag("VRPlayer") && other.CompareTag("Player")) ||
            (gameObject.CompareTag("Player") && other.CompareTag("VRPlayer")))
        {
            Debug.Log("Trigger between VRPlayer and Player detected.");
            ChangeSceneToGameOver();
        }
    }

    void ChangeSceneToGameOver()
    {
        Debug.Log("Changing scene to GameOverScene locally");
        SceneManager.LoadScene("GameOverScene");
    }
}
