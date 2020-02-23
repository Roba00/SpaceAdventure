using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesControl : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closeSprite;
    private BoxCollider2D boxColliderTop;
    private BoxCollider2D boxColliderBottom;
    private BoxCollider2D boxColliderMid;
    bool isWorking;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxColliderTop = gameObject.GetComponents<BoxCollider2D>()[0];
        boxColliderBottom = gameObject.GetComponents<BoxCollider2D>()[1];
        boxColliderMid = gameObject.GetComponents<BoxCollider2D>()[2];
        
        spriteRenderer.sprite = closeSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void open()
    {
        boxColliderTop.enabled = false;
        boxColliderBottom.enabled = false;
        boxColliderMid.enabled = false;
        spriteRenderer.sprite = openSprite;
    }

    void close()
    {
        boxColliderTop.enabled = true;
        boxColliderBottom.enabled = true;
        boxColliderMid.enabled = true;
        spriteRenderer.sprite = closeSprite;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet" && !isWorking)
        {
            StartCoroutine(startOpen());
            isWorking = false;
        }
    }

    IEnumerator startOpen()
    {
        isWorking = true;
        open();
        yield return new WaitForSeconds(3f);
        close();
        isWorking = false;
    }
}
