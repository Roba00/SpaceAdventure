using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBatControl : MonoBehaviour
{
    public GameObject player;

    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite noWingsSprite;
    public Sprite areaSprite;
    public Sprite lineSprite;
    public Sprite popSprite;

    private Transform tf;
    private bool isLeftSide;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    
    private bool canDamage;
    private bool isDamaging;
    private bool isAttacking;
    private float health;
    
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        tf = gameObject.GetComponent<Transform>();
        isLeftSide = false;      
        minX = 18f;
        maxX = 38f;
        minY = 0f;
        maxY = 7f;

        canDamage = true;
        isDamaging = false;
        isAttacking = false;
        health = 10f;
    }

    void Update()
    {
        StartCoroutine(AttackChooser());
        StartCoroutine(DeathCheck());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet" && !isDamaging)
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

        float width = Mathf.Abs(enemyX - playerX);
        float height = Mathf.Abs(enemyY - playerY);
        float angle = Mathf.Atan2(height, width) * Mathf.Rad2Deg;
        float hypotenuse = height/Mathf.Sin((angle*Mathf.PI)/180);
    }

    IEnumerator AttackChooser()
    {
        yield return new WaitForEndOfFrame();
    }

    IEnumerator InstantTransmission(int numberOfTimes)
    {
        yield return new WaitForEndOfFrame();
    }

    IEnumerator BreathFire(int numberOfFires)
    {
        yield return new WaitForEndOfFrame();
    }

    IEnumerator SwoopAttack()
    {
        yield return new WaitForEndOfFrame();
    }

    IEnumerator StartThunderbolt()
    {
        yield return new WaitForEndOfFrame();
    }

    IEnumerator Damage(float amount)
    {
        if (canDamage)
        {
            isDamaging = true;
            health -= amount;
            Color tempDamageColor = spriteRenderer.color;
                tempDamageColor.a = 0.75f;
                tempDamageColor.b = 0f;
                tempDamageColor.g = 0f;
                spriteRenderer.color = tempDamageColor;
            yield return new WaitForSeconds(1f);
            Color normalColor = spriteRenderer.color;
                normalColor.a = 1f;
                normalColor.b = 255f;
                normalColor.g = 255f;
                spriteRenderer.color = normalColor;
            isDamaging = false;
        }
    }

    IEnumerator DeathCheck()
    {
        if (health <= 0)
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}