using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Thomas Buffard
 * @Date 2020/02/14
 * 
 * This class respawns the character when they fall
*/

public class Respawn : MonoBehaviour
{
    // Fields
    /// <summary> Fall height </summary>
    [Tooltip("The height below the level that the player should reset at if they fall below")]
    [SerializeField] private float fallHeight = -14.0f;

    /// <summary> A rigidbody component on the player to control physics (serialized) </summary>
    [Tooltip("A rigidbody component on the player to control physics")]
    [SerializeField] private Rigidbody player;

    /// <summary>
    /// This method moves the player back to the starting position
    /// and resets their movement if they fall below fallHeight
    /// </summary>
    /// 
    /// 
    /// <returns> Returns void </returns>
    void Update()
    {
        // Check if the player has fallen 
        if (player.transform.position.y < fallHeight) 
        {
            // Move the player back to the origin and reset their movement
            player.transform.position = Vector3.zero;
            player.velocity = Vector3.zero;
        }
    }
}
