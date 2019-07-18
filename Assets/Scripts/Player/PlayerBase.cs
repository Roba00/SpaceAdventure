using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public GameObject Player;
    public GameObject Arm;
    public GameObject Propeller;
    public GameObject Head;
    public GameObject Body;
    public GameObject Bottom;

    private PlayerFlyControl PlayerFlyControl;
    private PlayerWalkControl PlayerWalkControl;
    private FlyAnim FlyAnim;
    private WalkAnim WalkAnim;
    
    public bool playerWalkOption;
    public bool playerFlyOption;

    void Start()
    {
        PlayerFlyControl = gameObject.GetComponent<PlayerFlyControl>();
        PlayerWalkControl = gameObject.GetComponent<PlayerWalkControl>();
        FlyAnim = Bottom.GetComponent<FlyAnim>();
        WalkAnim = Bottom.GetComponent<WalkAnim>();

        playerWalkOption = false;
        playerFlyOption = true;
    }

    void Update()
    {
        if (!playerWalkOption && playerFlyOption)
        {
            PlayerFlyControl.enabled = true;
            PlayerWalkControl.enabled = false;
            FlyAnim.enabled = true;
            WalkAnim.enabled = false;
        }
        else if (playerWalkOption && !playerFlyOption)
        {
            PlayerFlyControl.enabled = false;
            PlayerWalkControl.enabled = true;
            FlyAnim.enabled = false;
            WalkAnim.enabled = true;
        }
        else
        {
            Debug.Log("Error with player walk/fly options!");
        }
    }

    public IEnumerator SwitchToWalk()
    {
        yield return new WaitForSeconds(1f);
        playerFlyOption = true;
    }

    public IEnumerator SwitchToFly()
    {
        yield return new WaitForSeconds(1f);
        playerWalkOption  = false;
    }
}
