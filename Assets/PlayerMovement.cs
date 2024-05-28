using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    public FixedJoystick joystick;
    public float speed = 5.0f;

    void Update()
    {
        if (photonView.IsMine)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = Quaternion.Euler(0, transform.eulerAngles.y, 0) * direction;
                transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
            }
        }
    }
}
