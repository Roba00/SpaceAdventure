using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoiControl : MonoBehaviour
{
    public GameObject projectile;
    private SpriteRenderer spriteRenderer;
    public Sprite sprite1;
    public Sprite sprite2;
    private ParticleSystem particles;
    private Rigidbody2D rb;
    public GameObject player;

    private bool isAttackMode;
    private bool isDamaging;
    private bool isDying;
    private float health;
    private bool isRightPos;
    private int plyrDir;
    float plyrDistX, plyrDistY, plyrDist, plyrAngle;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        particles = gameObject.GetComponent<ParticleSystem>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = sprite1;
        StartCoroutine(Anim());
        isAttackMode = false;
        StartCoroutine(Attack());
        isDamaging = false;
        isDying = false;
        health = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDying)
        {
            GetDistances();
            FlipPosition();
            if (plyrDist< 8f) Move();
            StartCoroutine(DeathCheck());
            
            if (plyrDist < 10f && !isAttackMode) {isAttackMode = true; StartCoroutine(Attack());}
            if (plyrDist > 10f && isAttackMode /*&& !isAttacking*/) {isAttackMode = false; StopCoroutine(Attack());}
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

    void FlipPosition()
    {
        if (isRightPos)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else if (!isRightPos)
        {
            transform.localScale = new Vector3(-1,1,1);
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

        plyrDistX = playerX-enemyX;
        plyrDistY = playerY-enemyY;
        plyrAngle = Mathf.Atan2(plyrDistY, plyrDistX) * Mathf.Rad2Deg;
        plyrDist = Mathf.Abs(plyrDistY)/Mathf.Sin((plyrAngle*Mathf.Deg2Rad));
        if (playerX < enemyX) plyrDir = -1;
        else plyrDir = 1;

        if (enemyX > playerX) isRightPos = true;
        if (enemyX < playerX) isRightPos = false;
    }

    void Move()
    {
        Vector3 moveVector = new Vector3(plyrDir*2, rb.velocity.y, 0);
        rb.velocity = moveVector;
    }

    IEnumerator Attack()
    {
        yield return new WaitForEndOfFrame();
        while (isAttackMode)
        {
            for (int i = 0; i < 10; i++)
            {
                //Quaternion angle = new Quaternion().ToEulerAngles //(Vector3.AngleBetween(gameObject.transform.position, player.transform.position));
                GameObject lavaBall1 = Instantiate(projectile, transform.position, new Quaternion(0,0,0,0));
                float bulletSpeedX = Random.Range(100, 225);
                float bulletSpeedY = Random.Range(25, 200);
                lavaBall1.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(plyrDir*bulletSpeedX, bulletSpeedY*2.5f, 0));
                StartCoroutine(DeleteFireBallEleventually(lavaBall1));
                yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator Anim()
    {
        while (true)
        {
            spriteRenderer.sprite = sprite1;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.sprite = sprite2;
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator Damage(float amount)
    {
        isDamaging = true;
        health -= amount;
        particles.Play();
        Color tempDamageColor = spriteRenderer.color;
            tempDamageColor.a = 0.75f;
            tempDamageColor.r = 0f;
            tempDamageColor.b = 0f;
            spriteRenderer.color = tempDamageColor;
        yield return new WaitForSeconds(0.25f);
        Color normalColor = spriteRenderer.color;
            normalColor.a = 1f;
            normalColor.r = 255f;
            normalColor.b = 255f;
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

    IEnumerator DeleteFireBallEleventually(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        Destroy(obj);
    }
}
