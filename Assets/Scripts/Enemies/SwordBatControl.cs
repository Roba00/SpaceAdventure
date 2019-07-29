using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBatControl : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject player;
    private float width;
    private float height;
    private float angle;
    private float hypotenuse;
    private bool isRightPos;
    private bool shouldMove;
    private bool shouldJump;
    private bool shouldAttack;
    public bool isGrounded;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetDistances();
        StartCoroutine(Move());
        StartCoroutine(Jump());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground") isGrounded = true;
    }

    void GetDistances()
    {
        // height is opposite, width is adjacent
        // width, height, and hypotenuse, are the distances between the enemy and player
        float enemyX = transform.position.x;
        float enemyY = transform.position.y;
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        width = Mathf.Abs(enemyX - playerX);
        height = Mathf.Abs(enemyY - playerY);
        angle = Mathf.Atan2(height, width) * Mathf.Rad2Deg;
        hypotenuse = height/Mathf.Sin((angle*Mathf.PI)/180);

        if (enemyX > playerX) isRightPos = true;
        if (enemyX < playerX) isRightPos = false;

        if (width > 2.5f) shouldMove = true;
        else shouldMove = false;

        if (hypotenuse > 4f) shouldJump = true;
        else shouldJump = false;
    }

    IEnumerator Move()
    {
        float moveSpeed = 0.005f;
        yield return new WaitForEndOfFrame();
        while (shouldMove && isGrounded)
        {
            if (isRightPos)
            {
                Vector3 movement = new Vector3(-moveSpeed, 0, 0);
                transform.Translate(movement);
                yield return new WaitForSeconds(0.5f);
            }
            else if (!isRightPos)
            {
                Vector3 movement = new Vector3(moveSpeed, 0, 0);
                transform.Translate(movement);
                yield return new WaitForSeconds(0.5f);
            }
        }    
    }

    IEnumerator Jump()
    {
        float jumpPower = 5f;
        Vector3 jumpForce = new Vector3(0, jumpPower);
        yield return new WaitForEndOfFrame();
        if (shouldJump)
        {
            if (isGrounded)
            {
                isGrounded = false;
                rb.AddForce(Vector2.zero);
                rb.AddForce(jumpForce, ForceMode2D.Impulse);
                yield return new WaitForSeconds(10f);
            }
            yield return new WaitForSeconds(10f);
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForEndOfFrame();
    }

    IEnumerator Animation()
    {
        yield return new WaitForEndOfFrame();
    }
}
