using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AndroidConnectionManager : MonoBehaviourPunCallbacks
{
    public TMP_Text connectionStatusText;
    public string collectablesJsonFileName = "collectables.json"; // JSON file name

    private CollectableData collectableData;

    void Start()
    {
        connectionStatusText.text = "Connecting to server...";
        PhotonNetwork.ConnectUsingSettings();
        LoadCollectables(); // Load collectables data
    }

    void LoadCollectables()
    {
#if UNITY_EDITOR
        // Load directly from StreamingAssets folder in Unity Editor
        string filePath = Path.Combine(Application.streamingAssetsPath, collectablesJsonFileName);
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            collectableData = JsonUtility.FromJson<CollectableData>(dataAsJson);
            Debug.Log("Collectables loaded successfully.");
        }
        else
        {
            Debug.LogError("Cannot find " + collectablesJsonFileName + " file.");
        }
#else
        // Load via UnityWebRequest on device
        StartCoroutine(LoadCollectablesFromDevice());
#endif
    }

    IEnumerator LoadCollectablesFromDevice()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, collectablesJsonFileName);

        // Use UnityWebRequest for cross-platform file handling
        UnityWebRequest request = UnityWebRequest.Get(filePath);

        // Wait until the request is done
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load " + collectablesJsonFileName + ": " + request.error);
        }
        else
        {
            string dataAsJson = request.downloadHandler.text;
            collectableData = JsonUtility.FromJson<CollectableData>(dataAsJson);
            Debug.Log("Collectables loaded successfully.");
        }
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
        SpawnPlayer();
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        connectionStatusText.text = $"Disconnected: {cause}";
    }

    void SpawnPlayer()
    {
        if (collectableData == null || collectableData.possiblePositions == null || collectableData.possiblePositions.Count == 0)
        {
            Debug.LogError("No collectable data found or possible positions list is empty.");
            return;
        }

        // Shuffle the list of possible positions
        List<Position> shuffledPositions = new List<Position>(collectableData.possiblePositions);
        ShuffleList(shuffledPositions);

        // Select a random position from the shuffled list
        Position spawnPosition = shuffledPositions[Random.Range(0, shuffledPositions.Count)];

        // Spawn player at the selected position
        Vector3 playerSpawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z);
        PhotonNetwork.Instantiate("PlayerPrefab", playerSpawnPosition, Quaternion.identity);
    }

    void ShuffleList(List<Position> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            Position temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}

[System.Serializable]
public class CollectableData
{
    public List<Position> possiblePositions;
}

[System.Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}
