using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBatControl : MonoBehaviour
{
    private ParticleSystem particles;
    private Rigidbody2D rb;
    private SpriteRenderer animRenderer;
    private BoxCollider2D normalCollider;
    private BoxCollider2D strikeCollider;
    public GameObject player;
   
    private float width;
    private float height;
    private float angle;
    private float hypotenuse;
    
    private bool shouldMove;
    private bool shouldJump;
    private bool shouldAttack;
    
    private bool isRightPos;
    private bool isGrounded;
    private bool isDamaging;
    private bool isDying;

    public Sprite BatJumpAnim;
    public Sprite BatLeftFoot;
    public Sprite BatRightFoot;
    public Sprite BatStance;
    public Sprite BatStrike;

    private float health;


    void Start()
    {
        isDying = false;
        particles = gameObject.GetComponent<ParticleSystem>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animRenderer = gameObject.GetComponent<SpriteRenderer>();
        normalCollider = gameObject.GetComponents<BoxCollider2D>()[0];
        strikeCollider = gameObject.GetComponents<BoxCollider2D>()[1];
        normalCollider.enabled = true;
        strikeCollider.enabled = false;
        animRenderer.sprite = BatStance;
        isRightPos = false;
        isGrounded = true;
        shouldMove = false;
        shouldJump = false;
        shouldAttack = false;
        health = 3;
    }

    void Update()
    {
        if (!isDying)
        {
            GetDistances();

            if (shouldMove)
            {
                StartCoroutine(Move());
                StartCoroutine(WalkAnimation());
            }

            if (shouldJump && isGrounded)
            {
                StartCoroutine(Jump());
            }

            if (shouldAttack)
            {
                StartCoroutine(Attack());
            }
            else
            {
                normalCollider.enabled = true;
                strikeCollider.enabled = false;
            }

            StartCoroutine(DeathCheck());
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground") isGrounded = true;

        //if (col.gameObject.tag == "Bullet") Damage(col.gameObject.GetComponent<BulletInfo>().damage);
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.gameObject.tag == "Bullet" && !isDamaging && !isDying)
        { 
            StartCoroutine(Damage(col.gameObject.GetComponent<BulletInfo>().damage));
            Destroy(col.gameObject);
        }
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

        const float moveFollowDistance = 10f;
        const float jumpFollowDistance = 20f;
        const float attackFollowDistance = 1f;

        if (width <= moveFollowDistance && hypotenuse > attackFollowDistance) shouldMove = true;
        else shouldMove = false;

        if (hypotenuse <= jumpFollowDistance && hypotenuse > attackFollowDistance) shouldJump = true;
        else shouldJump = false;

        if (hypotenuse <= attackFollowDistance) shouldAttack = true;
        else shouldAttack = false;
    }

    IEnumerator Move()
    {
        float moveSpeed = 0.05f;
        if (isRightPos)
        {
            transform.localScale = new Vector3(1,1,1);
            Vector3 movement = new Vector3(-moveSpeed, 0, 0);
            transform.Translate(movement);
            yield return new WaitForSeconds(0.5f);
        }
        else if (!isRightPos)
        {
            transform.localScale = new Vector3(-1,1,1);
            Vector3 movement = new Vector3(moveSpeed, 0, 0);
            transform.Translate(movement);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator WalkAnimation()
    {
        float waitTime = 0.15f;
        while (true)
        {
            animRenderer.sprite = BatLeftFoot;
            yield return new WaitForSeconds(waitTime);
            animRenderer.sprite = BatRightFoot;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator Jump()
    {
        float jumpPower = 5f;
        float waitTime = Random.Range(3f, 6f);
        Vector3 jumpForce = new Vector3(0, jumpPower);
        animRenderer.sprite = BatJumpAnim;
        if (shouldJump && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector2.zero);
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(waitTime);
    }

    IEnumerator Attack()
    {
        normalCollider.enabled = false;
        strikeCollider.enabled = true;
        while (true)
        {
            animRenderer.sprite = BatStrike;
            yield return new WaitForSeconds(1.5f);
            animRenderer.sprite = BatStance;
            yield return new WaitForSeconds(1.5f);
        }

    }

    IEnumerator Damage(float amount)
    {
        isDamaging = true;
        health -= amount;
        particles.Play();
        Color tempDamageColor = animRenderer.color;
            tempDamageColor.a = 0.75f;
            tempDamageColor.b = 0f;
            tempDamageColor.g = 0f;
            animRenderer.color = tempDamageColor;
        yield return new WaitForSeconds(0.25f);
        Color normalColor = animRenderer.color;
            normalColor.a = 1f;
            normalColor.b = 255f;
            normalColor.g = 255f;
            animRenderer.color = normalColor;
        isDamaging = false;
    }

    IEnumerator DeathCheck()
    {
        if (health <= 0)
        {
            isDying = true;
            Destroy(normalCollider);
            Destroy(strikeCollider);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Color deadColor = new Color(255, 255, 255 , 0);
            deadColor.a = 1.1f;
            for (int i = 0; i < 11; i++)
            {
                deadColor.a -= 0.1f;
                animRenderer.color = deadColor;
                yield return new WaitForSeconds(0.1f);

            }
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
