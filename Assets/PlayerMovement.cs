using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.3f;

    private CharacterController controller;
    private Animator animator;
    private Transform cam;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        GroundCheck();
        Move();
        Jump();
        ApplyGravity();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h,0,v).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg
                + cam.eulerAngles.y;

            float angle = Mathf.LerpAngle(
                transform.eulerAngles.y,
                targetAngle,
                rotationSpeed * Time.deltaTime);

            transform.rotation =
                Quaternion.Euler(0,angle,0);

            Vector3 moveDir =
                Quaternion.Euler(0,targetAngle,0)
                * Vector3.forward;

            float speed = Input.GetKey(KeyCode.LeftShift)
                ? runSpeed
                : walkSpeed;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            animator.SetFloat(
                "speed",
                Input.GetKey(KeyCode.LeftShift)
                ? 1f
                : 0.5f);
        }
        else
        {
            animator.SetFloat("speed",0);
        }
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y =
                Mathf.Sqrt(jumpHeight * -2f * gravity);

            animator.SetTrigger("Jump");
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}