using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5.0f;
    private Joystick joystick;

    private CharacterController controller;

    void Start()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<CharacterController>();
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 1.7f, 0); // Adjust the camera position

            // Find the Joystick in the scene
            joystick = FindObjectOfType<Joystick>();
        }
        else
        {
            // Destroy the local camera if this is not the local player
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
    }

    void Update()
    {
        if (photonView.IsMine && joystick != null)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0, vertical);
            direction = Camera.main.transform.TransformDirection(direction);
            direction.y = 0;

            controller.Move(direction * speed * Time.deltaTime);
        }
    }
}
