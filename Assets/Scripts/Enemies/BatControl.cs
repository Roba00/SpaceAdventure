﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatControl : MonoBehaviour
{
    private ParticleSystem particles;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public GameObject player;
    public Sprite spriteUp;
    public Sprite spriteMiddle;
    public Sprite spriteDown;

    private bool isAttackMode;
    private bool isDamaging;
    private bool isDying;
    private float health;
    private bool isRightPos;
    private bool isAttacking;
    private int plyrDir;
    float plyrDistX, plyrDistY, plyrDist, plyrAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        particles = gameObject.GetComponent<ParticleSystem>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = spriteUp;
        StartCoroutine(anim());
        isAttackMode = false;
        StartCoroutine(Attack());
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
            
            if (plyrDist < 10f && !isAttackMode) {isAttackMode = true; StartCoroutine(Attack());}
            if (plyrDist > 10f && isAttackMode /*&& !isAttacking*/) {isAttackMode = false; StopCoroutine(Attack());}
        }
    }

    IEnumerator anim()
    {
        while (true)
        {
            spriteRenderer.sprite = spriteUp;
            yield return new WaitForSeconds(0.4f);
            spriteRenderer.sprite = spriteMiddle;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.sprite = spriteDown;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.sprite = spriteMiddle;
            yield return new WaitForSeconds(0.25f);
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
            transform.localScale = new Vector3(5,5,5);
        }
        else if (!isRightPos)
        {
            transform.localScale = new Vector3(-5,5,5);
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

    IEnumerator Attack()
    {
        while (isAttackMode)
        {
            Vector3 diveDown = new Vector3(plyrDir*2, -4f, 0);
            Vector3 riseUp = new Vector3(plyrDir*2, 4f, 0);
            isAttacking = true;
            rb.velocity = diveDown;
            yield return new WaitForSeconds(0.6f);
            rb.velocity = riseUp;
            yield return new WaitForSeconds(0.6f);
            rb.velocity = Vector3.zero;
            isAttacking = false;
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
}
