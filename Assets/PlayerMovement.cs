using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5.0f;
    public Joystick joystick;
    public float lookSpeed = 2.0f;

    private CharacterController controller;
    private Camera playerCamera;
    private Animator animator;

    private float cameraPitch = 0.0f;

    void Start()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();
            animator = GetComponent<Animator>();

            // Find the Joystick in the scene
            joystick = FindObjectOfType<Joystick>();
        }
        else
        {
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
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = transform.TransformDirection(direction); // Use local transform to move forward

        controller.Move(direction * speed * Time.deltaTime);
    }

    private void HandleLook()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width / 2)
            {
                float touchHorizontal = touch.deltaPosition.x * lookSpeed * Time.deltaTime;
                float touchVertical = -touch.deltaPosition.y * lookSpeed * Time.deltaTime;

                // Rotate the player horizontally
                transform.Rotate(0, touchHorizontal, 0);

                // Pivot the camera vertically
                cameraPitch = Mathf.Clamp(cameraPitch + touchVertical, -90, 90);
                playerCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);
            }
        }
    }

    private void UpdateAnimator()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Debug.Log($"Updating Animator: MoveX = {horizontal}, MoveY = {vertical}");

        animator.SetFloat("MoveX", horizontal);
        animator.SetFloat("MoveY", vertical);
    }
}
