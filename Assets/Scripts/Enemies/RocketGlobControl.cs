using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketGlobControl : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private ParticleSystem particles;
    public GameObject player;
    public GameObject gun;
    public GameObject projectile;
    public Sprite sprite1;
    public Sprite sprite2;

    private int plyrDir;
    private bool isRightPos;
    private float maxHealth = 3;
    private float health;
    private bool isAttackMode;
    private bool isAttacking;
    private bool isDamaging;
    private bool isDying;
    private float plyrDist;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        particles = gameObject.GetComponent<ParticleSystem>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(Anim());
        StartCoroutine(AnimMove());
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        GetDistances();
        if (!isDying)
        {
            if (plyrDist < 10f) isAttackMode = true;
            else isAttackMode = false;

            if (isAttackMode)
            {
                Follow();
                FaceAngle();
                AimControl();
                if (!isAttacking)
                {
                    StartCoroutine(Shoot());
                }
            }
            StartCoroutine(DeathCheck());
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

    void GetDistances()
    {
        // height is opposite, width is adjacent
        // width, height, and hypotenuse, are the distances between the enemy and player
        float enemyX = transform.position.x;
        float enemyY = transform.position.y;
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        float plyrDistX = playerX-enemyX;
        float plyrDistY = playerY-enemyY;
        angle = Mathf.Atan2(plyrDistY, plyrDistX) * Mathf.Rad2Deg;
        plyrDist = Mathf.Abs(plyrDistY)/Mathf.Sin((angle*Mathf.Deg2Rad));
        if (playerX < enemyX) plyrDir = -1;
        else plyrDir = 1;

        if (enemyX > playerX) isRightPos = true;
        if (enemyX < playerX) isRightPos = false;
    }

    IEnumerator AnimMove()
    {
        while (true)
        {
            gameObject.transform.Translate(0,0.1f,0);
            yield return new WaitForSeconds(0.25f);
            gameObject.transform.Translate(0,-0.1f,0);
            yield return new WaitForSeconds(0.25f);
        }
    }

    void AimControl()
    {
        Quaternion rotation;
        if (isRightPos)
            rotation = Quaternion.AngleAxis(angle-180, Vector3.forward);
        else
            rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        gun.transform.rotation = rotation;
    }

    void Follow()
    {
        float followX = plyrDir*10;
        float followY = 2f;
        float followSpeed = 0.0025f;
        Vector3 wantedPos = new Vector3(player.transform.position.x + followX, player.transform.position.y + followY, 0);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, wantedPos, followSpeed);
        transform.position = smoothPosition;
    }

    void FaceAngle()
    {
        if (isRightPos)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)*-1, transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator Anim()
    {
        while (true)
        {
            spriteRenderer.sprite = sprite1;
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.sprite = sprite2;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Shoot()
    {
        isAttacking = true;
        GameObject projectileInst;
        if (isRightPos)
        {
            projectileInst = Instantiate(projectile, gun.transform.position, gun.transform.rotation);
            projectileInst.transform.localScale = new Vector3(-1, 1, 1);
            projectileInst.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.left*300);
        }
        else
        {
            projectileInst = Instantiate(projectile, gun.transform.position, gun.transform.rotation);
            projectileInst.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right*300);
        }
        StartCoroutine(DeleteBulletEleventually(projectileInst));
        yield return new WaitForSeconds(1.5f);
        isAttacking = false;
    }

    IEnumerator SwoopDown()
    {
        yield return new WaitForEndOfFrame();
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
            gun.GetComponent<Rigidbody2D>().gravityScale = 1f;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }

    IEnumerator DeleteBulletEleventually(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        Destroy(obj);
    }
}
