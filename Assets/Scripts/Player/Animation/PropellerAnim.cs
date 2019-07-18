using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerAnim : MonoBehaviour
{
    public Sprite propeller1;
    public Sprite propeller2;
    private SpriteRenderer propellerRender;
    private float animWaitTime;

    void Start()
    {
        animWaitTime = 0.15f;
        propellerRender = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        while (true)
        {
            propellerRender.sprite = propeller1;
            yield return new WaitForSeconds(animWaitTime);
            propellerRender.sprite = propeller2;
            yield return new WaitForSeconds(animWaitTime);
        }
    }
}
