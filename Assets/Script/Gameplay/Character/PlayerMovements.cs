using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Header("Movement")]
    public float movSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readytoJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public float startYScale;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded = true;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    private Rigidbody rb;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    void Start()
    {
        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        myInput();
        SpeedControl();
        StateHandler();

        //handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void StateHandler()
    {
        
        if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            movSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            movSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    public void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if(Input.GetKey(jumpKey) && readytoJump && grounded)
        {
            readytoJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    public void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on Slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * movSpeed * 20f, ForceMode.Force);
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //on Ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * movSpeed * 10f, ForceMode.Force);
        }
        //in air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * movSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
    }

    public void SpeedControl()
    {
        //limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > movSpeed)
            {
                rb.velocity = rb.velocity.normalized * movSpeed;
            }
        }
        //limit speed on other (ground and air)
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity
            if (flatVel.magnitude > movSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * movSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        
    }

    public void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump()
    {
        readytoJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
