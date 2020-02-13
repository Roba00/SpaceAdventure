using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleControl : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite spriteUp;
    public Sprite spriteMiddle;
    public Sprite spriteDown;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteUp;
        StartCoroutine(anim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator anim()
    {
        while (true)
        {
            spriteRenderer.sprite = spriteUp;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = spriteMiddle;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = spriteDown;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
