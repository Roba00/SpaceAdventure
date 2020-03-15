using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBatControl : MonoBehaviour
{
    public GameObject player;
    public Slider slider;
    private PolygonCollider2D collider;

    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Sprite flapSprite;
    public Sprite noWingsSprite;
    public Sprite areaSprite;
    public Sprite lineSprite;
    public Sprite popSprite;
    public GameObject projectile;

    private Transform tf;
    
    bool playerAtBossArea;
    private bool isAttacking;
    private bool canDamage;
    private bool isDamaging;
    private bool isDying;

    private float maxHealth;
    private float health;
    private float plyrDist;

    private bool isRightPos;
    private float plyrAngle;
    
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        collider = gameObject.GetComponent<PolygonCollider2D>();
        tf = gameObject.GetComponent<Transform>();

        canDamage = true;
        isDamaging = false;
        maxHealth = 15f;
        health = maxHealth;
        isAttacking = false;
        Debug.Log("Is attacking: " + isAttacking);
        StartCoroutine(AnimFlapping());
        slider.gameObject.SetActive(false);
    }

    void Update()
    {
        GetDistances();
        slider.value = ((float)(health)/maxHealth);
        if (plyrDist < 10f && !playerAtBossArea)
        {
            
            playerAtBossArea = true;
            slider.gameObject.SetActive(true);
        }

        if (!isDying)
        {
            if (appearDone && disappearDone && !isFlappingAnim) StartCoroutine(AnimFlapping());
            if (!isAttacking && playerAtBossArea){StartCoroutine(AttackChooser());}
            StartCoroutine(DeathCheck());
        }
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
        plyrAngle = Mathf.Atan2(height, width) * Mathf.Rad2Deg;
        float hypotenuse = height/Mathf.Sin((plyrAngle*Mathf.PI)/180);

        plyrDist = hypotenuse;

        if (enemyX > playerX) isRightPos = true;
        if (enemyX < playerX) isRightPos = false;
    }

    private bool isFlappingAnim;
    IEnumerator AnimFlapping()
    {
        isFlappingAnim = true;
        spriteRenderer.sprite = defaultSprite;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.sprite = flapSprite;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.sprite = defaultSprite;
        isFlappingAnim = false;
    }

    private int lastChoice = 0;
    int repeats;
    IEnumerator AttackChooser()
    {
        Debug.Log("BOSS ATTACK");
        isAttacking = true;
        int choice = Random.Range(1, 5);
        if (lastChoice == 0) choice = 1;

        if (lastChoice == choice) repeats++;
        else repeats = 0;
        while (repeats >= 3) 
        {
            int randomChoice = Random.Range(1, 5);
            if (Random.Range(1, 3) != lastChoice)
            {
                choice = randomChoice;
                repeats = 0;
            }
        }
        lastChoice = choice;

        switch (choice)
        {
            //Instant Transmission
            case 1:
                StartCoroutine(InstantTransmission(0.5f));
                while (!finishedTransmission) yield return new WaitForEndOfFrame();
                break;
            
            //Swoop Attack
            case 2:
                StartCoroutine(SwoopAttack());
                while (!swoopingDone) yield return new WaitForEndOfFrame();
                break;
            
            //Breath Fire
            case 3:
                StartCoroutine(BreathFire());
                while (!breathingDone) yield return new WaitForEndOfFrame();
                break;

            //Drop Fire
            case 4: 
                StartCoroutine(DropFire());
                while (!droppingDone) yield return new WaitForEndOfFrame();
                break;
        }
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private bool appearDone;
    IEnumerator Appear()
    {
        appearDone = false;
        spriteRenderer.sprite = popSprite;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = lineSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = areaSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = noWingsSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = defaultSprite;
        yield return new WaitForSeconds(0.1f);
        collider.enabled = true;
        appearDone = true;
    }

    private bool disappearDone;
    IEnumerator Disappear()
    {
        disappearDone = false;
        spriteRenderer.sprite = defaultSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = noWingsSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = areaSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = lineSprite;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = popSprite;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = null;
        collider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        disappearDone = true;
    }

    private bool finishedTransmission = true;
    IEnumerator InstantTransmission(float waitTimeBetween)
    {
        int numberOfTimes = Random.Range(1, 2);
        finishedTransmission = false;
        for (int i = 0; i < numberOfTimes; i++)
        {
            StartCoroutine(Disappear());
            while (!disappearDone) yield return new WaitForEndOfFrame();
            Vector3 pos = new Vector3(Random.Range(145, 159), Random.Range(4, 8.5f), 0);
            transform.localPosition = pos;
            StartCoroutine(Appear());
            while (!appearDone) yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(waitTimeBetween);
        }
        finishedTransmission = true;
    }

    private bool swoopingDone = true;
    IEnumerator SwoopAttack()
    {
        swoopingDone = false;
        Vector3 target = player.transform.position;
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 0.15f);
            yield return new WaitForEndOfFrame();
        }
        swoopingDone = true;
    }

    bool breathingDone;
    IEnumerator BreathFire()
    {
        breathingDone = false;
        GameObject projectileInst;
        if (isRightPos)
        {
            projectileInst = Instantiate(projectile, gameObject.transform.position, Quaternion.AngleAxis(plyrAngle, Vector3.forward));
            projectileInst.transform.localScale = new Vector3(-1, 1, 1);
            projectileInst.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.left*500);
        }
        else
        {
            projectileInst = Instantiate(projectile, gameObject.transform.position, Quaternion.AngleAxis(-plyrAngle, Vector3.forward));
            projectileInst.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right*500);
        }
        StartCoroutine(DeleteBulletEleventually(projectileInst));
        yield return new WaitForSeconds(1f);
        breathingDone = true;
    }

    bool droppingDone;
    IEnumerator DropFire()
    {
        float[] xPos = {144.8f, 146.51f, 148.28f, 150.09f, 151.74f, 153.48f, 155.29f, 156.94f};
        float yPos = 7f;
        droppingDone = false;
        int randomNotChoose = Random.Range(1, 7);
        StartCoroutine(Disappear());
        while (!disappearDone) yield return new WaitForEndOfFrame();
        disappearDone = false;
        for (int i = 0; i<8; i++)
        {
            if (i != randomNotChoose)
            {
                GameObject projectileInst;
                Vector3 position = new Vector3(xPos[i], yPos, 0);
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0,0,-90f);
                projectileInst = Instantiate(projectile, position, rotation);
                projectileInst.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right*500);
                StartCoroutine(DeleteBulletEleventually(projectileInst));
                yield return new WaitForSeconds(0.35f);
            }
            if (i == randomNotChoose)
            {
                GameObject projectileInst;
                Vector3 position = new Vector3(xPos[i], yPos, 0);
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0,0,-90f); 
                projectileInst = Instantiate(projectile, position, rotation);
                projectileInst.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.25f);
                projectileInst.tag = "Untagged";  
                projectileInst.GetComponent<PolygonCollider2D>().enabled = false;
                projectileInst.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.right*500);
                StartCoroutine(DeleteBulletEleventually(projectileInst));
                yield return new WaitForSeconds(0.35f);
            }
        }
        yield return new WaitForSeconds(1f);
        disappearDone = true;
        StartCoroutine(Appear());
        while (!appearDone) yield return new WaitForEndOfFrame();
        droppingDone = true;
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
            isDying = true;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }

    IEnumerator DeleteBulletEleventually(GameObject obj)
    {
        yield return new WaitForSeconds(4f);
        Destroy(obj);
    }
}