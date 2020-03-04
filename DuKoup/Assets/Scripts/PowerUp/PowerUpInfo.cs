using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpInfo
{

    [SerializeField]
    public Vector3 position = new Vector3(0, 0, 0);

    [SerializeField]
    public Quaternion rotation = Quaternion.identity;

    public enum POWER_TYPE
    {
        Low_Gravity,
        Fast_Speed,
        Grabable,
        High_Jump,
        Double_Jump,
        Default
    }

    [SerializeField]
    private POWER_TYPE currentPowerType = POWER_TYPE.Default;

    public POWER_TYPE CURRENT_TYPE
    {
        get
        {
            return currentPowerType;
        }
        set
        {
            currentPowerType = value;
        }
    }
}
