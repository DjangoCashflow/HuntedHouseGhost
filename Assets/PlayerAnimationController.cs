using UnityEngine;
using Photon.Pun;

public class PlayerAnimatorController : MonoBehaviourPun
{
    private Animator animator;
    private float speed = 0f;
    private bool isAttacking = false;
    private bool isHit = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            HandleMovement();
            HandleAttack();
            HandleHit();
            UpdateAnimator();
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        speed = movement.magnitude;

        transform.Translate(movement * Time.deltaTime * 5f); // Adjust speed as necessary
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Example attack input
        {
            isAttacking = true;
            // Trigger attack logic
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isAttacking = false;
        }
    }

    void HandleHit()
    {
        if (Input.GetKeyDown(KeyCode.H)) // Example hit input
        {
            isHit = true;
            // Trigger hit logic
        }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            isHit = false;
        }
    }

    void UpdateAnimator()
    {
        animator.SetFloat("speed", speed);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isHit", isHit);
    }
}
