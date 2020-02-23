using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerCollectables : MonoBehaviour
{
    // Fields
    private int coinInventory = 0;
    [SerializeField][Tooltip("Insert here the UI to display the score of this player")] TextMeshProUGUI coinCountText;

    void OnTriggerEnter(Collider other){

        if (other.gameObject.CompareTag("Coin"))
        {
            coinInventory++;
            other.gameObject.SetActive(false);
            CoinManager.instance.IncreaseCoinTotal();
            SetCountText();
        }
    }
    // Display Score
    void SetCountText()
    {
        coinCountText.text = coinInventory.ToString();
    }

}
