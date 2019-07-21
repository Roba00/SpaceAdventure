using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    private float followSpeed;

    void Start()
    {
        followSpeed = 0.025f;
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 newPosition = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        Vector3 smoothPositioning = Vector3.Lerp(transform.position, newPosition,followSpeed);
        transform.position = smoothPositioning;
    }
}