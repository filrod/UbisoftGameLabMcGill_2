using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Rikke Aas
 * @Date 2020/01/31
 * 
 * This class controls the planking behaviour for the player with plaking ability
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

    private void Plank()
    {
        isPlanking = true;
        plankingSpace.GetComponent<PlankingSpace>().TurnOnBridge(); // Turns on bridge collider of planking space
        
        positionBeforePlank = transform.position; // Storing the position of the player to be able to move it back after planking is done

        // The following three lines probably won't stay like this when we get animations.
        // Mostly just for visuals, the other player doesn't actually use this player as a bridge (it is a sepparate collider)
        transform.position = new Vector3(plankingSpace.transform.position.x, 0, 0);
        transform.eulerAngles = new Vector3(0, 0, 90);
        transform.localScale = new Vector3(0.1f, 4f, 1f);
    }

    private void UnPlank()
    {
        isPlanking = false;
        plankingSpace.GetComponent<PlankingSpace>().TurnOffBridge(); // Turning of bridge collider of the planking space
        
        // For the following three lines: see comments for similar lines in Plank()
        transform.position = positionBeforePlank;
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
        
        positionBeforePlank = Vector3.zero; // Resetting the postion (shouldn't be necessaary, but just in case)
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlankingSpace")) // Detecting when player enters the planking space
        {
            Debug.Log("Entered planking space");
            inPlankinggSpace = true;
            plankingSpace = other.gameObject; // Storing reference to the planking space, this lets player turn on and off bridge when planking
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlankingSpace")) // Detecting when player leaves planking space
        {
            Debug.Log("Leaving planking space");
            inPlankinggSpace = false;
            plankingSpace = null;
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (!isPlanking) return; // Only want to ignore collision when player is planking (can only happen in planking space)

        if (collisionInfo.gameObject.CompareTag("Player")) // Only ignoring collisions between players
        {
            Physics.IgnoreCollision(collisionInfo.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    public bool PlayerIsPlanking()
    {
        return isPlanking;
    }
    
}
