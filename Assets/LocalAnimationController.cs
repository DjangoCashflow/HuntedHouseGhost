using UnityEngine;
using Photon.Pun;

public class LocalAnimationController : MonoBehaviourPunCallbacks
{
    private Animator animator;
    private Vector3 lastPosition;
    [SerializeField] private float speedThreshold = 0.1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
        }
        lastPosition = transform.position;
    }

    void Update()
    {
        // Only update animations if this is not the local player
        if (!photonView.IsMine)
        {
            UpdateAnimation();
        }
    }

    void UpdateAnimation()
    {
        Vector3 currentPosition = transform.position;
        float speed = Vector3.Distance(currentPosition, lastPosition) / Time.deltaTime;

        // Use the speedThreshold to determine if the character is moving
        bool isMoving = speed > speedThreshold;

        // Update the SpeedPlayer parameter
        animator.SetFloat("SpeedPlayer", isMoving ? speed : 0f);

        // Update last position for the next frame
        lastPosition = currentPosition;
    }
}