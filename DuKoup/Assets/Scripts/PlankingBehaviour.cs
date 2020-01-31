using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Rikke Aas
 * @Date 2020/01/30
 * 
 * This class controls the planking behaviour
*/

public class PlankingBehaviour : MonoBehaviour
{

    private bool inPlankinggSpace = false; // Variable to tell if a player is in a space where planking is available
    private GameObject plankingSpace; // Reference to the planking space, if not in a planking space, this will be null


    void FixedUpdate()
    {
        if (inPlankinggSpace && Input.GetKeyDown(KeyCode.P))
        {
            Plank();
        }
        if (inPlankinggSpace && Input.GetKeyUp(KeyCode.P))
        {
            UnPlank();
        }
    }

    private void UnPlank()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Plank()
    {
        // transform.position = plankingSpace.transform.GetChild(1).position;

        transform.eulerAngles = new Vector3(0, 0, 90);
        transform.localScale = new Vector3(0.1f, plankingSpace.transform.GetChild(2).position.x - plankingSpace.transform.GetChild(1).position.x, 1f);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (other.CompareTag("PlankingSpace"))
        {
            Debug.Log("Entered planking space");
            inPlankinggSpace = true;
            plankingSpace = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlankingSpace"))
        {
            Debug.Log("Leaving planking space");
            inPlankinggSpace = false;
            plankingSpace = null;
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (!inPlankinggSpace) return;

        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collisionInfo.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
    
}
