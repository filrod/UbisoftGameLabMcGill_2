using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Rikke Aas
 * @Date 2020/01/31
 * 
 * This abstract class is inherited by both planking scripts (player with planking ability and player without)
 * Methods in this class determine if players enter or exit a planking zone
*/

public abstract class PlankingSpaceDetection : MonoBehaviour
{
    protected bool inPlankingSpace = false; // Variable to tell if a player is in a space where planking is available
    protected GameObject plankingSpace; // Reference to the planking space, if not in a planking space, this will be null

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlankingSpace")) // Detecting when player enters the planking space
        {
            Debug.Log("Entered planking space");
            inPlankingSpace = true;
            plankingSpace = other.gameObject; // Storing reference to the planking space, this lets player turn on and off bridge when planking
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlankingSpace")) // Detecting when player leaves planking space
        {
            Debug.Log("Leaving planking space");
            inPlankingSpace = false;
            plankingSpace = null;
        }
    }
}
