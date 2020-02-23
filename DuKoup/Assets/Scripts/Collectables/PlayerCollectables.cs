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
    [SerializeField][Tooltip("Insert here the UI to display the winning message")] TextMeshProUGUI winningText;
    private bool canRestart = false;

    void OnTriggerEnter(Collider other){

        if (other.gameObject.CompareTag("Coin"))
        {
            coinInventory++;
            other.gameObject.SetActive(false);
            CoinManager.instance.IncreaseCoinTotal();
            SetCountText();
        }

        if (other.gameObject.CompareTag("End"))
        {
            winningText.SetText("You Won !");
            other.gameObject.SetActive(false);
            canRestart = true;
            
        }
    }
    // Display Score
    void SetCountText()
    {
        coinCountText.text = coinInventory.ToString();
    }

    void Update(){
        if (canRestart && (Input.GetKey(KeyCode.Escape))){
            Application.LoadLevel(0);
        }
    }
}
