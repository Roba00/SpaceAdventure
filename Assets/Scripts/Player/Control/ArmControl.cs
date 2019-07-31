using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
    public GameObject bullet;
    private Quaternion shootAngle;
    private float angleMinConstraint;
    private float angleMaxConstraint;
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
        AngleConstraints();
        MovementControl();
        ShootingControl();
    }

    void MovementControl()
    {
        if (Input.GetKey("d"))
        {
            transform.Rotate(new Vector3(0,0,armRotateSpeed));
        }
        if (Input.GetKey("s"))
        {
            transform.Rotate(new Vector3(0,0,-armRotateSpeed));
        }
    }
    
    void ShootingControl()
    {
        shootAngle = new Quaternion(0,0, transform.rotation.z, transform.rotation.w);

        if (Input.GetKey("space") && !isShootWaiting)
        {
            StartCoroutine(Shoot());
        }
    }

    void AngleConstraints()
    {
        if (gameObject.GetComponentInParent<PlayerBase>().IsRight())
        {
            angleMinConstraint = 5f;
            angleMaxConstraint = 165f;
            if (transform.eulerAngles.z < angleMinConstraint)
            {
                Vector3 newAngle = new Vector3(transform.rotation.x, 
                transform.rotation.y, angleMinConstraint);
                transform.eulerAngles = newAngle;
            }
            else if (transform.eulerAngles.z > angleMaxConstraint)
            {
                Vector3 newAngle = new Vector3(transform.rotation.x, 
                transform.rotation.y, angleMaxConstraint);
                transform.eulerAngles = newAngle;
            }
        }
        if (!gameObject.GetComponentInParent<PlayerBase>().IsRight())
        {
            angleMinConstraint = 355f;
            angleMaxConstraint = 195f;
            if (transform.eulerAngles.z > angleMinConstraint)
            {
                Vector3 newAngle = new Vector3(transform.rotation.x, 
                transform.rotation.y, angleMinConstraint);
                transform.eulerAngles = newAngle;
            }
            else if (transform.eulerAngles.z < angleMaxConstraint)
            {
                Vector3 newAngle = new Vector3(transform.rotation.x, 
                transform.rotation.y, angleMaxConstraint);
                transform.eulerAngles = newAngle;
            }
        }
    }

    IEnumerator Shoot()
    {
        StartCoroutine(ShootWaiting());
        GameObject localBullet = Instantiate(bullet, transform.position, shootAngle);
        localBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0, -shootDistance));
        localBullet.tag = "Bullet";
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
