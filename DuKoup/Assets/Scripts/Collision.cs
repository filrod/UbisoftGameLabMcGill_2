using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Robin Leman
 * @Date 2020/01/28
 * 
 * This class controls players collision and plane changes
*/
public class Collision : MonoBehaviour
{
    // Fields

    // Two players, instancePlayer has the script attached
    [SerializeField] private Rigidbody instancePlayer;
    [SerializeField] private Rigidbody otherPlayer;

    // Separate the level into 2 plains, with a distance diffPlane between the two planes. The initial plane should be the main one, alpha.
    [SerializeField] private float diffPlane;

    // Collision area attribute
    [SerializeField] private float collisionRadius;    // Should be the width of the player
    private float areaPositionMax;
    private float areaPositionMin;
    private float playerPos;

    [SerializeField] private float alphaPlane;
    [SerializeField] private float betaPlane;
    private float currentPlane;


    void Update()
    {

        // Create a collision area for otherPlayer
        // 2D area, with the radius of the size of the players, centered on otherPlayer.
            // Be sure that every players are in Alpha plane
        // If instancePlayer collides with collision area of otherPlayer, while he is in this area he moves to Beta plane
        // When instancePlayer goes out of the collision area, he moves back to alpha plane

        areaPositionMax = otherPlayer.transform.position.x + collisionRadius;
        areaPositionMin = otherPlayer.transform.position.x - collisionRadius;
        playerPos = instancePlayer.transform.position.x;
        currentPlane = instancePlayer.transform.position.z;

        if ( currentPlane == alphaPlane && (playerPos >= areaPositionMin && playerPos <= areaPositionMax))
        {

            instancePlayer.transform.position += new Vector3(0, 0, diffPlane); // Move player into beta Plane to avoid collision
        }

        if ( currentPlane == betaPlane && (playerPos <= areaPositionMin || playerPos >= areaPositionMax))
        {
            instancePlayer.transform.position += new Vector3(0, 0, -diffPlane); // Move player back into the main plane
        }
    }
}
