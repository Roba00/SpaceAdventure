using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnim : MonoBehaviour
{
    public float waitStartTime = 0;
    private float timeStep;
    private float scaleMultiplier;
    private int stepMultiplier;

    void Start()
    {
        timeStep = 0.1f;
        scaleMultiplier = 2.5f;
        stepMultiplier = 10;
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        yield return new WaitForSeconds(waitStartTime);
        while (true)
        {
            for (int i = 0; i < stepMultiplier; i++)
            {
                
                Vector3 scaleSmaller = new Vector3(transform.localScale.x - scaleMultiplier, 
                transform.localScale.y - scaleMultiplier,
                transform.localScale.z - scaleMultiplier);
                transform.localScale = scaleSmaller;
                yield return new WaitForSeconds(timeStep);
            }
            yield return new WaitForSeconds(timeStep);

            for (int i = 0; i < stepMultiplier; i++)
            {
                Vector3 scaleLarger = new Vector3(transform.localScale.x + scaleMultiplier,
                transform.localScale.y + scaleMultiplier,
                transform.localScale.z + scaleMultiplier);
                transform.localScale = scaleLarger;
                yield return new WaitForSeconds(timeStep);
            }
            yield return new WaitForSeconds(timeStep);
        }
    }
}
