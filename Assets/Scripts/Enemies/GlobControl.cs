using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobControl : MonoBehaviour
{
    private ParticleSystem particles;
    private SpriteRenderer spriteRenderer;
    public Sprite globNorm;
    public Sprite globCrouch;
    private float jumpVelocity;
    public GameObject player;
    float plyrDistX, plyrDistY, plyrDist, plyrAngle;
    int plyrDir;
    private Rigidbody2D rb;

    private bool canDamage;
    private bool isDamaging;
    private bool isAttacking;
    private float health;
    private bool isRightPos;
    private bool isDying;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        particles = gameObject.GetComponent<ParticleSystem>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = globNorm;
        StartCoroutine(StillAnimate());
        StartCoroutine(Attack());
        rb = gameObject.GetComponent<Rigidbody2D>();
        isDamaging = false;
        isDying = false;
        health = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDying)
        {
            GetDistances();
            FlipPosition();
            StartCoroutine(DeathCheck());
            
            if(isGrounded && !isAttacking && plyrDist < 10)
            {
                StartCoroutine(Attack());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet" && !isDamaging && !isDying)
        { 
            StartCoroutine(Damage(col.gameObject.GetComponent<BulletInfo>().damage));
            Destroy(col.gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void FlipPosition()
    {
        if (isRightPos)
        {
            transform.localScale = new Vector3(2,2,2);
        }
        else if (!isRightPos)
        {
            transform.localScale = new Vector3(-2,2,2);
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

        plyrDistX = Mathf.Abs(enemyX - playerX);
        plyrDistY = Mathf.Abs(enemyY - playerY);
        plyrAngle = Mathf.Atan2(plyrDistY, plyrDistX) * Mathf.Rad2Deg;
        plyrDist = plyrDistY/Mathf.Sin((plyrAngle*Mathf.PI)/180);
        if (playerX < enemyX) plyrDir = -1;
        else plyrDir = 1;

        if (enemyX > playerX) isRightPos = true;
        if (enemyX < playerX) isRightPos = false;
    }

    IEnumerator StillAnimate()
    {
        Vector3 moveDist = new Vector3(.05f, 0, 0);
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                transform.Translate(moveDist);
                yield return new WaitForSeconds(0.1f);
            }
            for (int i = 0; i < 10; i++)
            {
                transform.Translate(-moveDist);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        Vector3 velocity = new Vector3(plyrDir*4, 5f, 0);
        isGrounded = false;
        spriteRenderer.sprite = globCrouch;
        yield return new WaitForSeconds(.25f);
        rb.velocity = velocity;
        yield return new WaitForSeconds(.5f);
        spriteRenderer.sprite = globNorm;
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;
        isAttacking = false;
    }

    IEnumerator Damage(float amount)
    {
        isDamaging = true;
        health -= amount;
        particles.Play();
        Color tempDamageColor = spriteRenderer.color;
            tempDamageColor.a = 0.75f;
            tempDamageColor.b = 0f;
            tempDamageColor.g = 0f;
            spriteRenderer.color = tempDamageColor;
        yield return new WaitForSeconds(0.25f);
        Color normalColor = spriteRenderer.color;
            normalColor.a = 1f;
            normalColor.b = 255f;
            normalColor.g = 255f;
            spriteRenderer.color = normalColor;
        isDamaging = false;
    }

    IEnumerator DeathCheck()
    {
        if (health <= 0)
        {
            isDying = true;
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Color deadColor = new Color(255, 255, 255 , 0);
            deadColor.a = 1.1f;
            for (int i = 0; i < 11; i++)
            {
                deadColor.a -= 0.1f;
                spriteRenderer.color = deadColor;
                yield return new WaitForSeconds(0.1f);

            }
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
