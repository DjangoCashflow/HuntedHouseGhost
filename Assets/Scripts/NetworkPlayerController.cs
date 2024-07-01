using Photon.Pun;
using UnityEngine;

public class NetworkPlayerController : MonoBehaviourPun
{
    public float speed = 5.0f;
    public float lookSpeed = 2.0f;

    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            // Disable main camera to avoid duplicate view in VR player
            if (Camera.main != null)
            {
                Camera.main.gameObject.SetActive(false);
            }
        }
        else
        {
            // Destroy the camera for other players to avoid rendering issues
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
            HandleLook();
            UpdateAnimator();
        }
    }

    private void HandleMovement()
    {
        // This function would be empty or customized for your VR player.
        // Here we simulate movement for demonstration purposes.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = transform.TransformDirection(direction); // Use local transform to move forward

        controller.Move(direction * speed * Time.deltaTime);
    }

    private void HandleLook()
    {
        // This function would be empty or customized for your VR player.
        // Here we simulate look for demonstration purposes.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the player horizontally
        transform.Rotate(0, mouseX * lookSpeed, 0);

        // Pivot the camera vertically
        if (Camera.main != null)
        {
            float cameraPitch = Camera.main.transform.localEulerAngles.x - mouseY * lookSpeed;
            Camera.main.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
        }
    }

    private void UpdateAnimator()
    {
        // This function updates the Animator component based on the movement direction
        Vector3 velocity = controller.velocity;
        float moveSpeed = new Vector3(velocity.x, 0, velocity.z).magnitude;

        animator.SetFloat("speed", moveSpeed);

        if (velocity != Vector3.zero)
        {
            animator.SetFloat("MoveX", velocity.x / moveSpeed);
            animator.SetFloat("MoveY", velocity.z / moveSpeed);
        }
        else
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
        }
    }
}
