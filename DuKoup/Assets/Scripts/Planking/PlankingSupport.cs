using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Rikke Aas
 * @Date 2020/01/31
 * 
 * This class controls the behaiour related to planking of the non-planking player
 * ie. being able to pull the planker up on either of the sides (although it makes more sense to do so on the side they didn't start on) 
*/

public class PlankingSupport : PlankingSpaceDetection
{
    [SerializeField] private PlankingBehaviour plankingBehaviour; // Reference to the script held by the planking player

    void Update()
    {
        // Can only "pull" player up if they are in the planking space, and the other player is currently planking
        if (inPlankingSpace && plankingBehaviour.PlayerIsPlanking() && Input.GetKeyDown(KeyCode.O))
        {
            Vector3 moveToPos = transform.position;
            int direction = (transform.position.x < plankingBehaviour.transform.position.x) ? 1 : -1;
            moveToPos += new Vector3(direction, 0, 0); // Moving the planking player to a position just next to them
            plankingBehaviour.MovePlayerToPosition(moveToPos);
        }
    }
}
