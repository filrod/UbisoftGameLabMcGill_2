using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UBISOFT GAMES LAB - McGill Team #2
/// -----------------------------------
/// @author Robin Leman
/// @Date 2020/01/28
///
/// This class controls players collision and plane changes
/// </summary>

public class PlayersCollision : MonoBehaviour
{
    // Fields
    [SerializeField] private PlankingBehaviour plankingBehaviour;
    private PlayerManager playerManager;

    // Two players, instancePlayer has the script attached
    [SerializeField] private Rigidbody instancePlayer;
    [SerializeField] private Rigidbody otherPlayer;
    [SerializeField] private bool isMainPlayer;     // This should be Player 1, it is in case of a tie.

    // Collision area attribute
    [SerializeField] private float collisionRadius = 2f;    // Should be the width of the player

    private float areaPositionMax;
    private float areaPositionMin;
    private float playerPos;
    
    // Separate the level into 2 planes, with a distance diffPlane between the two planes. The initial plane should be the main one, alpha.
    [SerializeField] private float alphaPlane = 0;
    [SerializeField] private float betaPlane = 2;

    private float diffPlane;
    private float currentPlane;

    // Only the player with the greatest velocity should change plane
    private bool isFaster;
    private bool isAtEqualSpeed;

    void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        instancePlayer = GetComponent<Rigidbody>();
        plankingBehaviour = GetComponent<PlankingBehaviour>();
        if (playerManager.playerId == 1)
        {
            isMainPlayer = true;
        }
        diffPlane = betaPlane - alphaPlane;
    }

    /// <summary>
    /// UBISOFT GAMES LAB - McGill Team #2
    /// -----------------------------------
    /// @author Robin Leman
    /// @Date 2020/01/28
    ///
    /// This methods computes the distance between the players to prevent them from colliding.
    /// </summary>
    void Update()
    {
        if (plankingBehaviour.PlayerIsPlanking()) return;
        if (this.collisionRadius == 0f) return;
        // if (plankingBehaviour.PlayerIsPlanking()) return;
        if (otherPlayer == null) { 
        
            if (playerManager.OtherPlayer == null)
            {
                return;
            }
            otherPlayer = playerManager.OtherPlayer.GetComponentInChildren<Rigidbody>();
        }

        if (otherPlayer == null || instancePlayer == null) return;
        if (Mathf.RoundToInt(instancePlayer.transform.position.y) != Mathf.RoundToInt(otherPlayer.transform.position.y)) return;
        
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

        // If is in alpha plane, goes faster than other player and in radius: move to beta plane 
        // Be careful that the other player z is not the betaplane !
        if ( currentPlane == alphaPlane && (playerPos >= areaPositionMin && playerPos <= areaPositionMax) && isFaster && otherPlayer.transform.position.z != betaPlane )
        {
            // If they are at the same speed, Player 2 should move around Player 1
            if (!(isAtEqualSpeed))
            {
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    instancePlayer.transform.position += new Vector3(0, 0, diffPlane); // Move player into beta Plane to avoid collision
                }
                else
                {
                    this.otherPlayer.transform.position += new Vector3(0, 0, diffPlane);
                }
            }
        }
        else if ( currentPlane == betaPlane && (playerPos <= areaPositionMin || playerPos >= areaPositionMax))
        {
            instancePlayer.transform.position += new Vector3(0, 0, -diffPlane); // Move player back into the main plane
        }
        else if ((playerPos <= areaPositionMin || playerPos >= areaPositionMax) && (currentPlane != alphaPlane)){
            instancePlayer.transform.position = new Vector3(instancePlayer.transform.position.x, instancePlayer.transform.position.y, alphaPlane);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }

    public bool isSomethingInBeta(){
        RaycastHit hit;
        Vector3 origin = new Vector3 (playerPos, 10f, betaPlane);
        Debug.DrawRay(origin, Vector3.down * 1000, Color.white);
        return (Physics.Raycast(origin, Vector3.down, out hit, 1000f, LayerMask.GetMask("Ignore Raycast")) );
    }

    public float GetCollisonRadius()
    {
        return this.collisionRadius;
    }

    public void SetCollisionRadius(float newRadius)
    {
        this.collisionRadius = newRadius;
    }
}
