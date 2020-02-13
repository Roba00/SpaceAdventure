using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    private float followSpeed;
    private float deadZoneY;

    void Start()
    {
        followSpeed = 0.2f;
        deadZoneY = 1f;
    }

    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 newPosition;
        float cameraY = player.transform.position.y + 1.25f;
        /*if (player.transform.position.y - transform.position.y > deadZoneY)
        {
            cameraY = player.transform.position.y + 1.25f;
        }
        else
        {
            cameraY = player.transform.position.y;
        }
        if (player.GetComponent<PlayerBase>().faceRight == true)
        {
            newPosition = new Vector3(player.transform.position.x + 0.665f, cameraY, transform.position.z);
        }
        else
        {
            newPosition = new Vector3(player.transform.position.x - 0.665f, cameraY, transform.position.z);
        }*/
        newPosition = new Vector3(player.transform.position.x, cameraY, transform.position.z);
        Vector3 smoothPositioning = Vector3.Lerp(transform.position, newPosition, followSpeed);
        transform.position = smoothPositioning;
    }
}