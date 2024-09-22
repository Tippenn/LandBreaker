using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    public Rigidbody rb;
    public PlayerMovements playerMovement;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    public float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    private bool sliding;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovements>();

        startYScale = playerObj.localScale.y;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slideKey) && (horizontalInput!= 0 || verticalInput!= 0))
        {
            StartSlide();
        }

        if(Input.GetKeyUp(slideKey) && sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (sliding)
        {
            SlidingMovement();
        }
    }

    public void StartSlide()
    {
        sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down*5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    public void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward*verticalInput + orientation.right*horizontalInput;

        //sliding normal
        if(!playerMovement.OnSlope() || rb.velocity.y > -0.1f)
        {
            Debug.Log("not on slope");
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        //sliding down a slope
        else
        {
            Debug.Log("on slope");
            rb.AddForce(playerMovement.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }
        

        if(slideTimer <= 0f)
        {
            StopSlide();
        }
    }

    public void StopSlide()
    {
        sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
