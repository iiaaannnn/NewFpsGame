using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    public float wallRunSpeed;
    public float slideSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    public bool isCrouching;
    public bool isSliding;
    public bool isWallRunning;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouch")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Double Jump")]
    bool canDoubleJump;
    float doubleJumpTimer;
    bool startDoubleJumpTimer;

    [Header("Others")]
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

  

    Rigidbody rb;

    public GameObject speedLines;
    //for gun sway
    public GameObject gun;
    bool isMoving;
    public Animator gunanim;


    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air,
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        isCrouching = false;
        isMoving = false;
        canDoubleJump = false;
        doubleJumpTimer = 0.2f;

        speedLines.SetActive(false);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        //check if grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        GunSway();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        //double jump timer
        if(startDoubleJumpTimer)
        {
            if(doubleJumpTimer > 0)
            {
                doubleJumpTimer -= Time.deltaTime;
            }
            else if (doubleJumpTimer <= 0)
            {
                startDoubleJumpTimer = false;
                doubleJumpTimer = 0.2f;
                canDoubleJump = true;
            }
        }

    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (grounded)
        {
            canDoubleJump = false;
        }


        //jump
        if (Input.GetButtonDown("Jump") && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
            startDoubleJumpTimer = true;

        }
        //double jump
        if (Input.GetButtonDown("Jump") && canDoubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            canDoubleJump = false;
        }

        //crouch
        if (Input.GetButtonDown("Crouch"))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            moveSpeed = crouchSpeed;
            isCrouching = true;

        }
        else if (Input.GetButtonUp("Crouch"))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

            isCrouching = false;
        }

        //check if moving
        if (horizontalInput != 0 || verticalInput != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void StateHandler()
    {
        //wallrun
        if(isWallRunning)
        {
            state = MovementState.wallrunning;

            desiredMoveSpeed = wallRunSpeed;
        }
        //sliding
        else if(isSliding)
        {
            state = MovementState.sliding;

            if(OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }
        }
        //crouch
        else if (Input.GetButtonDown("Crouch"))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        //sprint
        else if (Input.GetButton("Sprint") && grounded && !isCrouching)
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
            if(isMoving)
            {
                speedLines.SetActive(true);
            }
            else
            {
                speedLines.SetActive(false);
            }
        }
        //walk
        else if (!isCrouching && grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
            speedLines.SetActive(false);

        }
        //air
        else
        {
            state = MovementState.air;
        }

        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //slope movement
        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //turn off graviy when on slope
        if(!isWallRunning)
        {
            rb.useGravity = !OnSlope();
        }

    }

    private void SpeedControl()
    {
        //limit speed on slope
        if(OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        //limit speed on ground and air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        
    }

    private void Jump()
    {
        exitingSlope = true;

        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        exitingSlope = false;
        readyToJump = true;
    }

    private void GunSway()
    {
        if(isMoving)
        {
            gunanim.SetBool("IsMoving", true);
        }
        else
        {
            gunanim.SetBool("IsMoving", false);
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if(OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;

            }
            else
            {
                time += Time.deltaTime * speedIncreaseMultiplier;
            }
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }
}

