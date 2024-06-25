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
            joystick = FindObjectOfType<Joystick>();

            // Debug log to confirm components are found
            Debug.Log($"Controller: {controller != null}, Camera: {playerCamera != null}, Animator: {animator != null}, Joystick: {joystick != null}");
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
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        direction = transform.TransformDirection(direction);

        // Store the current y position
        float currentY = transform.position.y;

        // Apply movement
        controller.Move(direction * speed * Time.deltaTime);

        // Force the y position to remain constant
        Vector3 newPosition = transform.position;
        newPosition.y = currentY;
        transform.position = newPosition;

        // Debug log for movement input and position
        Debug.Log($"Movement Input - Horizontal: {horizontal}, Vertical: {vertical}");
        Debug.Log($"Position after move: {transform.position}");
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
        if (animator == null)
        {
            Debug.LogError("Animator is null! Make sure it's attached to the GameObject.");
            return;
        }

        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Apply a small deadzone to prevent tiny movements
        if (Mathf.Abs(horizontal) < 0.1f) horizontal = 0;
        if (Mathf.Abs(vertical) < 0.1f) vertical = 0;

        animator.SetFloat("MoveX", horizontal);
        animator.SetFloat("MoveY", vertical);

        // Debug log for animator parameters
        Debug.Log($"Setting Animator Parameters - MoveX: {horizontal}, MoveY: {vertical}");
        Debug.Log($"Actual Animator Parameters - MoveX: {animator.GetFloat("MoveX")}, MoveY: {animator.GetFloat("MoveY")}");

        // Optional: Set an "IsMoving" bool for transitions
        bool isMoving = horizontal != 0 || vertical != 0;
        animator.SetBool("IsMoving", isMoving);
        Debug.Log($"IsMoving: {isMoving}");
    }
}