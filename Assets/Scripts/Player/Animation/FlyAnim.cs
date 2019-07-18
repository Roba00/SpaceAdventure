using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAnim : MonoBehaviour
{
    public Sprite fireLow;
    public Sprite fireHigh;
    private SpriteRenderer fireRender;
    private float animWaitTime;

    void Start()
    {
        animWaitTime = 0.15f;
        fireRender = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        while (true)
        {
            fireRender.sprite = fireHigh;
            yield return new WaitForSeconds(animWaitTime);
            fireRender.sprite = fireLow;
            yield return new WaitForSeconds(animWaitTime);
        }
    }
}
