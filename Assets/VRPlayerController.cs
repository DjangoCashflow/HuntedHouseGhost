using UnityEngine;
using Photon.Pun;

public class VRPlayerController : MonoBehaviourPunCallbacks
{
    public Animator humanAnimator;
    public Transform humanTransform; // Reference to the human transform
    public float movementSpeedThreshold = 0.1f;

    private Vector3 previousPosition;
    private float movementSpeed;

    void Start()
    {
        if (humanAnimator == null)
        {
            Debug.LogError("Human Animator is not assigned!");
        }
        if (humanTransform == null)
        {
            Debug.LogError("Human Transform is not assigned!");
        }
        previousPosition = humanTransform.position;
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        // Calculate movement speed
        Vector3 currentPosition = humanTransform.position;
        movementSpeed = Vector3.Distance(currentPosition, previousPosition) / Time.deltaTime;
        previousPosition = currentPosition;

        // Update Animator parameters
        UpdateAnimatorParameters();
    }

    void UpdateAnimatorParameters()
    {
        if (movementSpeed > movementSpeedThreshold)
        {
            humanAnimator.SetFloat("Speed", movementSpeed);
        }
        else
        {
            humanAnimator.SetFloat("Speed", 0);
        }
    }
}
