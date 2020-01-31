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
    private bool isPlanking = false;
    private GameObject plankingSpace; // Reference to the planking space, if not in a planking space, this will be null
    private Vector3 positionBeforePlank;


    void FixedUpdate()
    {
        if (inPlankinggSpace && !isPlanking && Input.GetKeyDown(KeyCode.P))
        {
            Plank();
        }
        if (inPlankinggSpace && isPlanking && Input.GetKeyUp(KeyCode.P))
        {
            UnPlank();
        }
    }

    private void UnPlank()
    {
        isPlanking = false;
        Debug.Log(positionBeforePlank);
        transform.position = positionBeforePlank;
        positionBeforePlank = Vector3.zero;
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Plank()
    {
        isPlanking = true;
        Debug.Log(transform.position);
        positionBeforePlank = transform.position;
        transform.position = new Vector3(plankingSpace.transform.position.x, 0, 0);

        transform.eulerAngles = new Vector3(0, 0, 90);
        transform.localScale = new Vector3(0.1f, 4f, 1f);
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
