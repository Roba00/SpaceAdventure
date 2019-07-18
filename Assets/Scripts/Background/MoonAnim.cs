using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonAnim : MonoBehaviour
{
    public Sprite anim0;
    public Sprite anim1;
    public Sprite anim2;
    public Sprite anim3;
    public Sprite anim4;
    public Sprite anim5;
    public Sprite anim6;
    public Sprite anim7;
    private SpriteRenderer moon;
    private int animWaitTime;
    private float timeStep;

    void Start()
    {
        animWaitTime = 2;
        timeStep = 0.15f;
        moon = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        while (true)
        {
            moon.sprite = anim0;
            yield return new WaitForSeconds(animWaitTime);
            moon.sprite = anim1;
            yield return new WaitForSeconds(timeStep);
            moon.sprite = anim2;
            yield return new WaitForSeconds(timeStep);
            moon.sprite = anim3;
            yield return new WaitForSeconds(timeStep);
            moon.sprite = anim4;
            yield return new WaitForSeconds(timeStep);
            moon.sprite = anim5;
            yield return new WaitForSeconds(timeStep);
            moon.sprite = anim6;
            yield return new WaitForSeconds(timeStep);
            moon.sprite = anim7;
            yield return new WaitForSeconds(timeStep);
        }
    }
}
