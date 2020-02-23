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
    private bool isCrouching;
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
        Crouch();
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
    }

    void JumpControl()
    {
        if (Input.GetKeyDown("w") && isGrounded)
        {   
            if (!isCrouching) jumpVelocity = 10f;
            else jumpVelocity = 12f;
            isGrounded = false;
            playerRb.velocity = new Vector2 (playerRb.velocity.x, jumpVelocity);
        }

        if (playerRb.velocity.y > 0 && !isGrounded && !Input.GetKey("w"))
        {
            playerRb.velocity -= Vector2.up * jumpVelocity/10;
        }
    }

    void HorizantalMovementControl()
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
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
        }
    }

    public bool isInAir()
    {
        return !isGrounded;
    }

    void Crouch()
    {
        if (Input.GetKeyDown("s"))
        {
            if (!isCrouching)
            {
                isCrouching = true;
                speed = 300f;
                Arm.SetActive(false);
                Propeller.SetActive(false);
                Head.SetActive(false);
                Bottom.SetActive(false);
                boxCollider.enabled = false;
                circleCollider.enabled = false;
            }
            else
            {
                isCrouching = false;
                speed = 200f;
                tellShooterNotCrouching = true;
                Arm.SetActive(true);
                Propeller.SetActive(true);
                Head.SetActive(true);
                Bottom.SetActive(true);
                boxCollider.enabled = true;
                circleCollider.enabled = true;
            }
        }
    }
}
