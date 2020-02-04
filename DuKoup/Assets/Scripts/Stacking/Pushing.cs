using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushing : MonoBehaviour
{

    private bool playerOnHead = false;
    [SerializeField] private GameObject otherPlayer;
    [SerializeField] private float pushSpeed;

    // Update is called once per frame
    void Update()
    {
        
        if (!playerOnHead) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 xy = Input.mousePosition;
            
            otherPlayer.GetComponent<Rigidbody>().velocity += new Vector3(0,1,0) * pushSpeed;
        }
    }




    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        { 
            Debug.Log("Collided with player");
            if (transform.position.y < collisionInfo.gameObject.transform.position.y)
            {
                Debug.Log("Other player is over player");
                if (collisionInfo.gameObject.transform.position.x > transform.position.x  - 0.5f && collisionInfo.gameObject.transform.position.x < transform.position.x + 0.5f)
                {
                    Debug.Log("Player is in correct position");
                    playerOnHead = true;
                }
            }
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            playerOnHead = false;
        }
    }
}
