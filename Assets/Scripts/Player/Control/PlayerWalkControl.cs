using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkControl : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private float speed;
    private float jumpVelocity;
    private float fallVelocity;
    private bool isGrounded;
    private bool isJumping;
    private bool isCrouching;
    private bool isCharging;
    public bool tellShooterNotCrouching;
    public GameObject Arm, Propeller, Head, Bottom;

    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        speed = 200f;
        jumpVelocity = 1f;
        isGrounded = false;
        tellShooterNotCrouching = false;
    }

    void Update()
    {
        HorizantalMovementControl();
        JumpControl();
        Crouch();
    }

    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
            isCharging = false;
            if (!gameObject.GetComponent<PlayerBase>().isDeath())
                transform.eulerAngles = new Vector3(0,0,0);
        }
    }

    void JumpControl()
    {
        if (Input.GetKeyDown("w") && isGrounded)
        {   
            isJumping = true;
            if (!isCrouching) jumpVelocity = 9f;
            else jumpVelocity = 12f;
            isGrounded = false;
            playerRb.velocity = new Vector2 (playerRb.velocity.x, jumpVelocity);
        }
        else if (Input.GetKeyUp("w"))
        {
            isJumping = false;
        }

        if (Input.GetKey("w") &&  !isGrounded && !isCrouching && !isCharging && !isJumping)
        {
            isCharging = true;
            playerRb.velocity = new Vector3(0, 0, 0);
            if (transform.localScale.x == .75f) 
            {
                transform.eulerAngles = new Vector3(0,0,-90);
                playerRb.velocity = new Vector3(10f, -7.5f, 0);
            }
            if (transform.localScale.x == -.75f)
            {
                transform.eulerAngles = new Vector3(0,0,90);
                playerRb.velocity = new Vector3(-10f, -7.5f, 0);
            }
        }

        if (playerRb.velocity.y > 0 && !isGrounded && !Input.GetKey("w"))
        {
            playerRb.velocity -= Vector2.up * jumpVelocity/10;
        }
    }

    void HorizantalMovementControl()
    {
        if (!isCharging)
        {
            if (Input.GetKey("a"))
            {
                playerRb.velocity = new Vector2(-speed * Time.deltaTime, playerRb.velocity.y);
            }
            else if (Input.GetKey("d"))
            {
                playerRb.velocity = new Vector2(speed * Time.deltaTime, playerRb.velocity.y);
            }
            else
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x/1.1f, playerRb.velocity.y);
            }
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown("s"))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                speed = 300f;
                Arm.GetComponent<SpriteRenderer>().enabled = false;
                Propeller.GetComponent<SpriteRenderer>().enabled = false;
                Head.GetComponent<SpriteRenderer>().enabled = false;
                Bottom.GetComponent<SpriteRenderer>().enabled = false;
                boxCollider.enabled = false;
                circleCollider.enabled = false;
            }
            else
            {
                isCrouching = false;
                speed = 200f;
                tellShooterNotCrouching = true;
                Arm.GetComponent<SpriteRenderer>().enabled = true;
                Propeller.GetComponent<SpriteRenderer>().enabled = true;
                Head.GetComponent<SpriteRenderer>().enabled = true;
                Bottom.GetComponent<SpriteRenderer>().enabled = true;
                boxCollider.enabled = true;
                circleCollider.enabled = true;
            }
        }
    }

    public bool isInAir()
    {
        return !isGrounded;
    }

    public bool isChargeAttack()
    {
        return isCharging;
    }
}
