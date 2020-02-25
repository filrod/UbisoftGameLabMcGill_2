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

public class PlankingBehaviour : PlankingSpaceDetection
{
    private bool isPlanking = false;
    private Vector3 positionBeforePlank;

    void FixedUpdate()
    {
        if (inPlankingSpace && !isPlanking && Input.GetKeyDown(KeyCode.P))
        {
            Plank();
        }
        if (inPlankingSpace && isPlanking && Input.GetKeyUp(KeyCode.P))
        {
            UnPlank();
        }
    }

    /**
    * Method Name: Plank()
    * 
    * Description:
    * Method to make the player plank. It will also move the player into the center of the planking space and squash them
    * to look like a "bridge". The specifics will change when we get animations etc.
    * 
    * Calls
    * -----
    * SetBridgeActivity(..)
    */
    private void Plank()
    {
        isPlanking = true;
        SetBridgeActivity(true); // Turns on bridge collider of planking space
        
        positionBeforePlank = transform.position; // Storing the position of the player to be able to move it back after planking is done

        // The following three lines probably won't stay like this when we get animations.
        // Mostly just for visuals, the other player doesn't actually use this player as a bridge (it is a sepparate collider)
        transform.position = new Vector3(plankingSpace.transform.position.x, 0, 0);
        transform.eulerAngles = new Vector3(0, 0, 90);
        transform.localScale = new Vector3(0.1f, 4f, 1f);
    }

    /**
    * Method Name: UnPlank()
    * 
    * Description:
    * Method to take the player out of a plank, and move them back to their original position (position from where they started the plank)
    *
    * Calls
    * -----
    * SetBridgeActivity(..)
    */
    private void UnPlank()
    {
        isPlanking = false;
        SetBridgeActivity(false); // Turning of bridge collider of the planking space
        
        // For the following three lines: see comments for similar lines in Plank()
        transform.position = positionBeforePlank;
        transform.eulerAngles = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
        
        positionBeforePlank = Vector3.zero; // Resetting the postion (shouldn't be necessaary, but just in case)
    }

    /**
    * Method Name: MovePlayerToPosition(..)
    * 
    * Description:
    * Method other player will call to move planking player to other side after crossing planking bridge.
    * It will also un-plank the player (since this makes sense), even though the planking player might still be holding down 
    * the planking button.
    * 
    * Parameters
    * ----------
    * newPos : Vector3
    *       This variable is the position the planking player will be moved to
    *
    * Calls
    * -----
    * UnPlank()
    */
    public void MovePlayerToPosition(Vector3 newPos)
    {
        positionBeforePlank = newPos;
        UnPlank();
    }

    /**
    * Method Name: SetBridgeActivity(..)
    * 
    * Description:
    * Method used by planking player (PlankingBehaviour script) to turn on or off the "bridge" which
    * is just a collider to give the illusion of one player using the other as a bridge.
    * 
    * Parameters
    * ----------
    * activity : bool
    *       This variable tell the method wether the bridge should be actived (true) or deactivated (false)
    */
    public void SetBridgeActivity(bool activity)
    {
        // Child 0 of the planking space is the "bridge" (gameobject with only a collider attached)
        plankingSpace.transform.GetChild(0).gameObject.SetActive(activity);
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
