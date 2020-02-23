using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleControl : MonoBehaviour
{
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particles;
    private Rigidbody2D rb;
    public Sprite spriteUp;
    public Sprite spriteMiddle;
    public Sprite spriteDown;
    bool isRightPos;
    int dir;
    private bool isAttacking;
    private  bool isDamaging;
    private bool isDying;
    private float health;
    float width, height, angle, hypotenuse;

    public GameObject lavaBall;

    // Start is called before the first frame update
    void Start()
    {
        particles = gameObject.GetComponent<ParticleSystem>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteUp;
        health = 4f;
        StartCoroutine(anim());
        isDying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDying)
        {
            GetDistances();
            FacePlayer();   
            if (hypotenuse < 3f && !isAttacking)
            {
                StartCoroutine(attack());
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

    void FacePlayer()
    {
        if (isRightPos) transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z);
        if (!isRightPos) transform.localScale = new Vector3(-2, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator anim()
    {
        while (true)
        {
            spriteRenderer.sprite = spriteUp;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = spriteMiddle;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = spriteDown;
            yield return new WaitForSeconds(0.2f);
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

        if (enemyX > playerX) {isRightPos = true; dir = 1;}
        if (enemyX < playerX) {isRightPos = false; dir = -1;}
    }

    IEnumerator attack()
    {
        isAttacking = true;
        transform.localScale = new Vector3(2, -2, 2);

        //Move Up
        for (int i = 0; i < 50; i++)
        {
            transform.Translate(0, 0.1f, 0);
            yield return new WaitForSeconds(0.01f);
        }

        //Launch Lavaball
        for (int i = 0; i < 10; i++)
        {
            //Quaternion angle = new Quaternion().ToEulerAngles //(Vector3.AngleBetween(gameObject.transform.position, player.transform.position));
            GameObject lavaBall1 = Instantiate(lavaBall, transform.position, new Quaternion(0,0,0,0));
            float bulletSpeed = Random.Range(25, 325);
            lavaBall1.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(dir*-bulletSpeed, 0, 0));
            StartCoroutine(DeleteFireBallEleventually(lavaBall1));
            yield return new WaitForSeconds(0.1f);
        }

        //Move Down
        transform.localScale = new Vector3(2, 2, 2);
        for (int i = 0; i < 50; i++)
        {
            transform.Translate(0, -0.1f, 0);
            yield return new WaitForSeconds(0.005f);
        }
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

    IEnumerator DeleteFireBallEleventually(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        Destroy(obj);
    }
}
