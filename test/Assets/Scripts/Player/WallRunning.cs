using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WallRunning : MonoBehaviour //mine
{
    [Header("Wall Running")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    private float wallRunTimer;
    public float wallClimbSpeed;


    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovement playerMovement;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(playerMovement.isWallRunning)
        {
            WallRunningMovement();
        }
    }
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //wallrunning
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if(!playerMovement.isWallRunning)
            {
                StartWallRun();
            }

            if (wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }

            if(wallRunTimer < 0 && playerMovement.isWallRunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if(Input.GetButtonDown("Jump"))
            {
                WallJump();
            }
        }

        else if(exitingWall)
        {
            if(playerMovement.isWallRunning)
            {
                StopWallRun();
            }

            if(exitWallTimer > 0)
            {
                exitWallTimer -= -Time.deltaTime;
            }

            if(exitWallTimer <= 0)
            {
                exitingWall = false;
            }

        }

        else
        {
            if (playerMovement.isWallRunning)
            {
                StopWallRun();
            }
        }
        

    }


    private void StartWallRun()
    {
        playerMovement.isWallRunning = true;
        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //camera tilt
        //if(wallLeft)
        //{
        //    cam.DoTilt(-5f);
        //}
        //if (wallRight)
        //{
        //    cam.DoTilt(5f);
        //}

    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if(Input.GetButton("UpwardsRunning"))//upwardsRunning
        {
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        }
       

        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        //make graviy weaker
        if(useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }
    }

    private void StopWallRun()
    {
        playerMovement.isWallRunning = false;
        //cam.DoTilt(0f);
    }

    private void WallJump()
    {
        //exit wall
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
