using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingPowerUp : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerManager.SetThrow();
            gameObject.SetActive(false);
        }
    }
}
