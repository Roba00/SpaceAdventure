using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCounter : MonoBehaviour
{
    private int energyCount;

    void Start()
    {
        energyCount = 0;
    }

    public void EnergyAdd()
    {
        energyCount += 1;
    }

    public int EnergyCount()
    {
        return energyCount;
    }
}