using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sliding : MonoBehaviour
{
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement playerMovement;
    public CapsuleCollider playerCollider;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    public float startYScale;
    public float startYScaleCollider;

    private float horizontalInput;
    private float verticalInput;

    //private bool IsSliding;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
        startYScaleCollider = playerCollider.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Slide") && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if(Input.GetButtonUp("Slide") && playerMovement.isSliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if(playerMovement.isSliding)
        {
            SlidingMovement();
        }
    }
    private void StartSlide()
    {
        playerMovement.isSliding = true;
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        playerCollider.transform.localScale = new Vector3(playerCollider.transform.localScale.x, slideYScale, playerCollider.transform.localScale.z);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        if(slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        playerMovement.isSliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        playerCollider.transform.localScale = new Vector3(playerCollider.transform.localScale.x, startYScaleCollider, playerCollider.transform.localScale.z);


    }
}
 