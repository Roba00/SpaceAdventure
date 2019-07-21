using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyControl : MonoBehaviour
{
    public Rigidbody2D playerRb;
    private float force;
    private float incrementalForce;
    private float maxForce;
    private float normalSpeed;
    private float slowSpeed;
    private bool isBraking;
    private bool faceRight;

    void Start()
    {
        force = 0;
        incrementalForce = 2f;
        maxForce = 18f;
        normalSpeed = 0.1f;
        slowSpeed = 0.04f;
        isBraking = false;
        faceRight = true;
    }

    void Update()
    {
        BrakeControl();
        FlyControl();
        HorizantalMovementControl();
        FacingControl();
    }

    void HorizantalMovementControl()
    {
        if (Input.GetKey("left") && !isBraking)
        {
            //transform.Rotate(0,0,0.5f);
            transform.Translate(-normalSpeed, 0, 0);
        }
        else if (Input.GetKey("left") && isBraking)
        {
            transform.Translate(-slowSpeed, 0, 0);
        }

        if (Input.GetKey("right") && !isBraking)
        {
            //transform.Rotate(0,0,-0.5f);
            transform.Translate(normalSpeed, 0, 0);
        }
        else if (Input.GetKey("right") && isBraking)
        {
            transform.Translate(slowSpeed, 0, 0);
        }
    }

    void BrakeControl()
    {
        if (Input.GetKey("down"))
        {
            playerRb.velocity = Vector2.zero;
            force = 0;
            playerRb.gravityScale = 0;
            isBraking = true;
        }
        if (Input.GetKeyUp("down"))
        {
            playerRb.gravityScale = 1;
            isBraking = false;
        }
    }

    void FlyControl()
    {
        if (Input.GetKey("up") && !isBraking)
        {
            force += incrementalForce;
        }
        else if (Input.GetKeyUp("up") && !isBraking)
        {
            force = 0;
        }
        if (force > maxForce)
        {
            force = maxForce;
        }

        playerRb.AddForce(Vector2.up * force);
    }

    void FacingControl()
    {
        float offsetX = 1.5f;
        if (Input.GetKeyDown("left") && faceRight)
        {
            faceRight = false;
            Vector3 scaleBackwards = new Vector3(-transform.localScale.x, 
            transform.localScale.y, transform.localScale.z);
            Vector3 resetPosition = new Vector3(transform.localPosition.x + offsetX, 
            transform.localPosition.y, transform.localPosition.z);
            transform.localScale = scaleBackwards;
            transform.localPosition = resetPosition;
        }
        if (Input.GetKeyDown("right") && !faceRight)
        {
            faceRight = true;
            Vector3 scaleBackwards = new Vector3(-transform.localScale.x, 
            transform.localScale.y, transform.localScale.z);
            Vector3 resetPosition = new Vector3(transform.localPosition.x - offsetX, 
            transform.localPosition.y, transform.localPosition.z);
            transform.localScale = scaleBackwards;
            transform.localPosition = resetPosition;
        }
    }
}
