using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
    public GameObject bullet;
    private Quaternion shootAngle;
    private float armRotateSpeed;
    private float shootDistance;
    private float shootWaitTime;
    private bool isShootWaiting;
    
    void Start()
    {
        armRotateSpeed = 3;
        shootDistance = 125f;
        shootWaitTime = 1f;
        isShootWaiting = false;
    }

    void Update()
    { 
        shootAngle = new Quaternion(0,0, transform.rotation.z, transform.rotation.w);

        if (Input.GetKey("d"))
        {
            transform.Rotate(new Vector3(0,0,armRotateSpeed));
        }
        if (Input.GetKey("s"))
        {
            transform.Rotate(new Vector3(0,0,-armRotateSpeed));
        }

        if (Input.GetKey("space") && !isShootWaiting)
        {
            StartCoroutine(Shoot());
        }
    }
    
    IEnumerator Shoot()
    {
        StartCoroutine(ShootWaiting());
        GameObject localBullet = Instantiate(bullet, transform.position, shootAngle);
        localBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, -shootDistance));
        yield return new WaitForSeconds(3f);
        Destroy(localBullet);
    }

    IEnumerator ShootWaiting()
    {
        isShootWaiting = true;
        yield return new WaitForSeconds(shootWaitTime);
        isShootWaiting = false;
    }
}
