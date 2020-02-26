using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // Singleton 

    public static CoinManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Field
    private int coinTotal = 0;

    public void IncreaseCoinTotal(){
        coinTotal++;
    }
}
