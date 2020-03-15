using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFloorControl : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Anim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Anim()
    {
        while (true)
        {
            spriteRenderer.sprite = sprite1;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = sprite2;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = sprite3;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.sprite = sprite2;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
