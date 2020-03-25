using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
    public GameObject bullet;
    public GameObject missle;
    private Quaternion shootAngle;
    private float angleMinConstraint;
    private float angleMaxConstraint;
    private float bulletSpeed;
    private float shootWaitTime;
    private bool isShootWaiting;
    
    void Start()
    {
        bulletSpeed = 350f;
        shootWaitTime = 0.5f;
        isShootWaiting = false;
    }

    void Update()
    { 
        MovementControl();
        ShootingControl();
    }

    void MovementControl()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        Quaternion rotation = Quaternion.AngleAxis(angleZ, Vector3.forward);
        transform.rotation = rotation;
    }
    
    void ShootingControl()
    {
        shootAngle = new Quaternion(0,0, transform.rotation.z, transform.rotation.w);
        if (gameObject.GetComponentInParent<PlayerWalkControl>().tellShooterNotCrouching)
        {
            StopAllCoroutines();
            isShootWaiting = false;
            gameObject.GetComponentInParent<PlayerWalkControl>().tellShooterNotCrouching = false;
        }
        if (Input.GetKey(KeyCode.Mouse0) && !isShootWaiting)
        {
            StartCoroutine(ShootBullet());
        }
        if (Input.GetKey(KeyCode.Mouse1) && !isShootWaiting)
        {
            StartCoroutine(ShootMissle());
        }
    }

    IEnumerator ShootBullet()
    {
        StartCoroutine(BulletWaiting());
        GameObject localBullet = Instantiate(bullet, transform.position, shootAngle);
        localBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, -bulletSpeed));
        localBullet.tag = "Bullet";
        yield return new WaitForSeconds(1f);
        Destroy(localBullet);
    }

    IEnumerator ShootMissle()
    {
        StartCoroutine(MissleWaiting());
        GameObject localMissle = Instantiate(missle, transform.position, shootAngle);
        localMissle.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, -bulletSpeed*1.25f));
        localMissle.tag = "Bullet";
        yield return new WaitForSeconds(1f);
        Destroy(localMissle);
    }

    IEnumerator BulletWaiting()
    {
        isShootWaiting = true;
        yield return new WaitForSeconds(shootWaitTime);
        isShootWaiting = false;
    }

    IEnumerator MissleWaiting()
    {
        isShootWaiting = true;
        yield return new WaitForSeconds(shootWaitTime+0.75f);
        isShootWaiting = false;
    }
}
