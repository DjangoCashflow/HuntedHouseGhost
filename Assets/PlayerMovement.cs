using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5.0f;
    public Joystick joystick;
    public float lookSpeed = 2.0f;

    private CharacterController controller;
    private Camera playerCamera;

    void Start()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();
            playerCamera.transform.SetParent(transform);

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
        }
    }

    private void HandleMovement()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0;

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
                playerCamera.transform.Rotate(touchVertical, 0, 0);
            }
        }
    }
}
