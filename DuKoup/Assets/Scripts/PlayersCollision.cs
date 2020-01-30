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

public class PlayersCollision : MonoBehaviour
{
    // Fields

    // Two players, instancePlayer has the script attached
    [SerializeField] private Rigidbody instancePlayer;
    [SerializeField] private Rigidbody otherPlayer;
    [SerializeField] private bool isMainPlayer;     // This should be Player 1, it is in case of a tie.

    // Collision area attribute
    [SerializeField] private float collisionRadius;    // Should be the width of the player
    private float areaPositionMax;
    private float areaPositionMin;
    private float playerPos;
    
    // Separate the level into 2 planes, with a distance diffPlane between the two planes. The initial plane should be the main one, alpha.
    [SerializeField] private float alphaPlane;
    [SerializeField] private float betaPlane;
    private float diffPlane;
    private float currentPlane;

    //
    private static bool isSomeoneInBeta;

    // Only the player with the greatest velocity should change plane
    private bool isFaster;
    private bool isAtEqualSpeed;

    void Start()
    {
        diffPlane = betaPlane - alphaPlane;
    }

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

        isFaster = instancePlayer.velocity.magnitude > otherPlayer.velocity.magnitude;
        isAtEqualSpeed = instancePlayer.velocity.magnitude == otherPlayer.velocity.magnitude;

        if ( currentPlane == alphaPlane && (playerPos >= areaPositionMin && playerPos <= areaPositionMax) && isFaster && !isSomeoneInBeta)
        {   
            isSomeoneInBeta = true;

            // If they are at the same speed, Player 2 should move around Player 1
            if (! (isAtEqualSpeed && isMainPlayer)){
                instancePlayer.transform.position += new Vector3(0, 0, diffPlane); // Move player into beta Plane to avoid collision
            }
        }

        if ( currentPlane == betaPlane && (playerPos <= areaPositionMin || playerPos >= areaPositionMax))
        {
            instancePlayer.transform.position += new Vector3(0, 0, -diffPlane); // Move player back into the main plane
            isSomeoneInBeta = false;
        }
    }
}
