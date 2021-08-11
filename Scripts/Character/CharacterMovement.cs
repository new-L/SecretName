using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterAnimator animator;
    public CharacterController controller;
    [Header("Moving")]
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;
    private float horizontal;
    private float vertical;
    private Vector3 direction;
    private Vector3 moveDir;

    [Header("Jump")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpCooldown;

    [Header("Smoothing")]
    [SerializeField] private float turnSmoothTime = .1f;
    [SerializeField] private float turnSmoothVelocity;


    [Header("CameraMove")]
    public Transform camera;


    [Header("Gravity")]
    public float gravity;
    private Vector3 velocity;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask grounMask;
    [SerializeField] private bool isGrounded;



    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }

    private void Update()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, grounMask);
        if(IsGrounded && velocity.y < 0)
        {
            animator.isJumping = false;
            animator.isFalling = false;
            animator.isRuning = false;
            animator.isSprinting = false;
            velocity.y = -5f;
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (!Input.GetKey(KeyCode.LeftShift)) Run();
            if (Input.GetKey(KeyCode.LeftShift)) Sprint();
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Jump();
        }
        Falling();
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void Jump()
    {  
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        animator.isFalling = true;
    }

    private void Falling()
    {
        if (!IsGrounded && !animator.isJumping)
        {
            animator.isFalling = true;
        }
    }

    private void Run()
    {
        animator.isRuning = true;
        animator.isSprinting = false;
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }

    private void Sprint()
    {
        animator.isSprinting = true;
        animator.isRuning = false;
        controller.Move(moveDir.normalized * sprintSpeed * Time.deltaTime);        
    }
}
