using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyControl : MonoBehaviour
{
    public GameObject canvas;
    public GameObject energyUIPrefab;
    public EnergyCounter energyCounter;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            AddEnergyToUI();
        }
    }

    void AddEnergyToUI()
    {
        GameObject energyUI = Instantiate(energyUIPrefab);
        energyUI.transform.SetParent(canvas.transform);
        RectTransform energyUITransform = energyUI.GetComponent<RectTransform>();
        Vector3 newEnergyUIPosition = 
            new Vector3(375 - (30*energyCounter.EnergyCount()), 250f, 0);
        energyUITransform.localPosition = newEnergyUIPosition;
        energyUI.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
        energyCounter.EnergyAdd();
        Destroy(gameObject);
    }
}