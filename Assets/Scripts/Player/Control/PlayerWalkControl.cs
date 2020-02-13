using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkControl : MonoBehaviour
{
    public Rigidbody2D playerRb;
    private float speed;
    private float slowSpeed;
    private float jumpVelocity;
    private float fallVelocity;
    private bool isBraking;
    private bool isGrounded;
    private bool isJumping;
    private bool isNormalSpeed;

    void Start()
    {
        speed = 3f;
        slowSpeed = 0.5f;
        jumpVelocity = 1f; //350f
        isBraking = false;
        isGrounded = false;
        isJumping = false;
        isNormalSpeed = true;
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
        }

        Debug.Log(col.gameObject.name);
    }

    void JumpControl()
    {
        /*if (Input.GetKeyDown("w") && isGrounded)
        {
            isGrounded = false;
            isJumping = true;
            playerRb.AddForce(Vector2.zero);
            playerRb.velocity = new Vector2 (playerRb.velocity.x, jumpPower);
        }

        if (Input.GetKey("w") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                playerRb.velocity = new Vector2 (playerRb.velocity.x, jumpPower/2);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetKeyUp("w"))
        {
            jumpTimeCounter = 0;
            isJumping = false;
        }*/

        if (Input.GetKeyDown("w") && isGrounded)
        {   
            jumpVelocity = 10f;
            isGrounded = false;
            playerRb.velocity = new Vector2 (playerRb.velocity.x, jumpVelocity);
        }

        if (playerRb.velocity.y < 0 && !isGrounded && !Input.GetKey("w"))
        {
            //fallVelocity -= Physics2D.gravity.y/10000;
            //playerRb.velocity -= Vector2.up * fallVelocity;
        }

        if (playerRb.velocity.y > 0 && !isGrounded && !Input.GetKey("w"))
        {
            //fallVelocity -= Physics2D.gravity.y/10000;
            playerRb.velocity -= Vector2.up * jumpVelocity/10;
        }
    }

    void HorizantalMovementControl()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isNormalSpeed = !isNormalSpeed;
        }
        
        if (isNormalSpeed)
        {
            speed = 3f;
        }
        else
        {
            speed = 6f;
        }

        if (Input.GetKey("a") && !isBraking)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("a") && isBraking)
        {
            transform.Translate(-slowSpeed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKey("d") && !isBraking)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey("d") && isBraking)
        {
            transform.Translate(slowSpeed * Time.deltaTime, 0, 0);
        }
    }

    public bool isInAir()
    {
        return !isGrounded;
    }
}
