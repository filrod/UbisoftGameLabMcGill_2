using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    [SerializeField]
    public List<PowerUpSpot> powerUps;

    [SerializeField]
    PowerUpSpot powerUpPrefab;

    [SerializeField]
    public List<PowerUpInfo> powerUpInfos;

    private void Start()
    {
        powerUps.Clear();
        foreach (PowerUpInfo s in powerUpInfos)
        {
            PowerUpSpot p = Instantiate(powerUpPrefab, s.position, s.rotation);
            p.powerUpInfo = s;
            powerUps.Add(p);
        }
    }
}
