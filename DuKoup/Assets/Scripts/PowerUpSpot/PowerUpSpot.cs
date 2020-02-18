using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpot : MonoBehaviour
{
    /// <summary>
    /// The player who enter the power up spot
    /// </summary>
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(powerUpSpotCollider.isTrigger);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        // TODO: refactor the dummy, and unify the tag
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;

            // player power up
            Debug.Log("Player power Up");
        }
    }
}
