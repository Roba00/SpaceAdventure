using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlyControl : MonoBehaviour
{
    public SpriteRenderer[] playerSprites;
    public Rigidbody2D playerRb;
    private float force;
    private float incrementalForce;
    private float maxForce;
    private float normalSpeed;
    private float slowSpeed;
    private bool isBraking;
    private bool isInvincible;
    private int lives;

    void Start()
    {
        playerSprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        force = 0;
        incrementalForce = 2f;
        maxForce = 18f;
        normalSpeed = 0.1f;
        slowSpeed = 0.04f;
        isBraking = false;
        isInvincible = false;
        lives = 3;
    }

    void Update()
    {
        BrakeControl();
        FlyControl();
        HorizantalMovementControl();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && !isInvincible)
        {
            StartCoroutine(Invivility());
        }
        if (col.gameObject.tag == "Energy")
        {
            EnergyCollide(col.gameObject);
        }
        if (col.gameObject.tag == "Upgrade")
        {
            UpgradeCollide(col.gameObject);
        }
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

    IEnumerator Invivility()
    {
        isInvincible = true;
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < playerSprites.Length; i++)
            {
                Color tempInvisibility = playerSprites[i].color;
                tempInvisibility.a = 0.25f;
                tempInvisibility.b = 0f;
                tempInvisibility.g = 0f;
                playerSprites[i].color = tempInvisibility;
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < playerSprites.Length; i++)
            {
                Color tempVisibility = playerSprites[i].color;
                tempVisibility.a = 1f;
                tempVisibility.b = 255f;
                tempVisibility.g = 255f;
                playerSprites[i].color = tempVisibility;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
    }

    void EnergyCollide(GameObject energy)
    {

    }

    void UpgradeCollide(GameObject upgrade)
    {

    }
    
    void Damage(int amount)
    {

    }

    void Death()
    {

    }
}
