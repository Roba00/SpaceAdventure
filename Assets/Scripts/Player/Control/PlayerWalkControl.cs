using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkControl : MonoBehaviour
{
    public Rigidbody2D playerRb;
    private float normalSpeed;
    private float slowSpeed;
    private float jumpPower;
    private bool isBraking;
    private bool isGrounded;
    private bool isJumping;
    private float jumpTime;
    public float jumpTimeCounter;

    void Start()
    {
        normalSpeed = 3f;
        slowSpeed = 0.5f;
        jumpPower = 5f; //350f
        isBraking = false;
        isGrounded = false;
        isJumping = false;
        jumpTime = 1.5f;
        jumpTimeCounter = jumpTime;
    }

    void Update()
    {
        HorizantalMovementControl();
    }

    void FixedUpdate()
    {
        JumpControl();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumpTimeCounter = jumpTime;
        }

        Debug.Log(col.gameObject.name);
    }

    void JumpControl()
    {
        if (Input.GetKeyDown("up") && isGrounded)
        {
            isGrounded = false;
            isJumping = true;
            playerRb.AddForce(Vector2.zero);
            playerRb.velocity = new Vector2 (playerRb.velocity.x, jumpPower);
        }

        if (Input.GetKey("up") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                playerRb.velocity = new Vector2 (playerRb.velocity.x, jumpPower/2);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetKeyUp("up"))
        {
            jumpTimeCounter = 0;
            isJumping = false;
        }
    }

    void HorizantalMovementControl()
    {
        if (Input.GetKey("left") && !isBraking)
        {
            transform.Translate(-normalSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("left") && isBraking)
        {
            transform.Translate(-slowSpeed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey("right") && !isBraking)
        {
            transform.Translate(normalSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("right") && isBraking)
        {
            transform.Translate(slowSpeed * Time.deltaTime, 0, 0);
        }
    }
}
