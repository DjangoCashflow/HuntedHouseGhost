using Photon.Pun;
using UnityEngine;

public class PlayerLook : MonoBehaviourPun
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    void Start()
    {
        if (photonView.IsMine)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            HandleLookInput();
        }
    }

    private void HandleLookInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x > Screen.width / 2)
            {
                float mouseX = touch.deltaPosition.x * mouseSensitivity * Time.deltaTime;
                float mouseY = touch.deltaPosition.y * mouseSensitivity * Time.deltaTime;
                LookAround(mouseX, mouseY);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x > Screen.width / 2)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                LookAround(mouseX, mouseY);
            }
        }
    }

    private void LookAround(float mouseX, float mouseY)
    {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerBody.localRotation = Quaternion.Euler(xRotation, playerBody.localEulerAngles.y, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
