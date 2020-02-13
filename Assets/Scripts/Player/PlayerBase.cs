using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public SpriteRenderer[] playerSprites;
    private bool isInvincible;
    public GameObject Lives;
    private int lifeCount;
    public Sprite deathFace;

    public bool faceRight;

    void Start()
    {
        PlayerFlyControl = gameObject.GetComponent<PlayerFlyControl>();
        PlayerWalkControl = gameObject.GetComponent<PlayerWalkControl>();
        FlyAnim = Bottom.GetComponent<FlyAnim>();
        WalkAnim = Bottom.GetComponent<WalkAnim>();
        playerSprites = gameObject.GetComponentsInChildren<SpriteRenderer>();

        playerWalkOption = true;
        playerFlyOption = false;
        isInvincible = false;
        lifeCount = 3;
        faceRight = true;
    }

    void Update()
    {
        FacingControl();

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
        else if (!playerWalkOption && !playerFlyOption)
        {
            PlayerFlyControl.enabled = false;
            PlayerWalkControl.enabled = false;
            FlyAnim.enabled = false;
            WalkAnim.enabled = false;
        }
        else
        {
            Debug.Log("Error with player walk/fly options!");
        }

        if (lifeCount <= 0 && !(!playerFlyOption && !playerWalkOption))
        {
            StartCoroutine(Death());
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "NormalEnemy" && !isInvincible)
        {
            StartCoroutine(Invincibility());
            Damage(1);
        }
    }
    
    IEnumerator Invincibility()
    {
        isInvincible = true;
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < playerSprites.Length; i++)
            {
                Color tempInvisibility = playerSprites[i].color;
                tempInvisibility.a = 0.25f;
                tempInvisibility.b = 0f;
                tempInvisibility.g = 0f;
                playerSprites[i].color = tempInvisibility;
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < playerSprites.Length; i++)
            {
                Color tempVisibility = playerSprites[i].color;
                tempVisibility.a = 1f;
                tempVisibility.b = 255f;
                tempVisibility.g = 255f;
                playerSprites[i].color = tempVisibility;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
    }

    void EnergyCollide(GameObject energy)
    {

    }

    void UpgradeCollide(GameObject upgrade)
    {

    }

    void Damage(int amount)
    {
        GameObject damagedLife = 
        Lives.transform.GetChild(lifeCount-1).gameObject.transform.GetChild(0).gameObject;
        Destroy(damagedLife);
        lifeCount -= amount;
    }

    public IEnumerator Death()
    {
        playerWalkOption = false;
        playerFlyOption = false;
        gameObject.transform.Find("Head").
        GetComponent<SpriteRenderer>().sprite = deathFace;
        Vector3 deathRotation = new Vector3(0,0,-90);
        transform.Rotate(deathRotation);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

    void FacingControl()
    {
        float offsetX = 0f; //Was 1.7f
        if (Input.GetKeyDown("a") && faceRight)
        {
            faceRight = false;
            Vector3 scaleBackwards = new Vector3(-transform.localScale.x, 
            transform.localScale.y, transform.localScale.z);
            Vector3 resetPosition = new Vector3(transform.localPosition.x + offsetX, 
            transform.localPosition.y, transform.localPosition.z);
            transform.localScale = scaleBackwards;
            transform.localPosition = resetPosition;
            //Vector3 newArmRotation = new Vector3(0,0,10/*transform.Find("Arm").transform.rotation.z*/);
            //transform.Find("Arm").transform.localEulerAngles = newArmRotation;
        }
        if (Input.GetKeyDown("d") && !faceRight)
        {
            faceRight = true;
            Vector3 scaleBackwards = new Vector3(-transform.localScale.x, 
            transform.localScale.y, transform.localScale.z);
            Vector3 resetPosition = new Vector3(transform.localPosition.x - offsetX, 
            transform.localPosition.y, transform.localPosition.z);
            transform.localScale = scaleBackwards;
            transform.localPosition = resetPosition;
            //Vector3 newArmRotation = new Vector3(0,0,10/*transform.Find("Arm").transform.rotation.z*/);
            //transform.Find("Arm").transform.localEulerAngles = newArmRotation;
        }
    }

    public bool IsRight()
    {
        return faceRight;
    }
}
