using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float aimSpeed = 2f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.3f;

    [Header("Aim")]
    public GameObject crosshair;
    public bool isAiming;

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

        if (crosshair != null)
            crosshair.SetActive(false);
    }

    void Update()
    {
        Aim();

        GroundCheck();

        Move();

        Jump();

        FloorGrab();

        WallGrab();

        ApplyGravity();

        Shoot();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
        groundCheck.position,
        groundDistance,
        groundMask);

        animator.SetBool("Grounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        velocity.y = -2f;
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg
                + cam.eulerAngles.y;

            if (isAiming)
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.Euler(0, cam.eulerAngles.y, 0),
                    rotationSpeed * Time.deltaTime);
            }
            else
            {
                float angle = Mathf.LerpAngle(
                    transform.eulerAngles.y,
                    targetAngle,
                    rotationSpeed * Time.deltaTime);

                transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            Vector3 moveDir =
                Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            float speed;

            if (isAiming)
            {
                speed = aimSpeed;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            if (isAiming)
            {
                animator.SetFloat("speed", 0.5f);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetFloat("speed", 1f);
            }
            else
            {
                animator.SetFloat("speed", 0.5f);
            }
        }
        else
        {
            animator.SetFloat("speed", 0f);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            animator.SetTrigger("Jump");
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Aim()
    {
        isAiming = Input.GetMouseButton(1);

        animator.SetBool("isAim", isAiming);

        if (crosshair != null)
            crosshair.SetActive(isAiming);
    }

    void Shoot()
    {
       if (isAiming && Input.GetMouseButtonDown(0))
       {
           animator.SetTrigger("shoot");
       } 
    }

    void FloorGrab()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("GrabFloor");
        }
    }

    void WallGrab()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("GrabWall");
        }
    }
}