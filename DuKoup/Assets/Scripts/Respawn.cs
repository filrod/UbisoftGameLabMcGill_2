using Photon.Pun;
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

    public float FallHeight{
        get 
        {
            return fallHeight;
        }
        set
        {
            fallHeight = value;
        }
    }

    /// <summary> Other player </summary>
    [Tooltip("The player to follow when you respawn")][HideInInspector]
    [SerializeField] private GameObject otherPlayer;

    /// <summary>
    /// The max velocity the player can move while dead
    /// </summary>
    [SerializeField]
    [Tooltip("Player movenment max velocity when dead")]
    [Range(0f, 0.5f)]
    private float maxVelocity_dead = 0.05f;

    [SerializeField]
    [Tooltip("Occilation frquency multiplier for Sine bobing of dead enemy")]
    [Range(0f, 50f)]
    private float occilationFreq = 20f;

    /// <summary> A rigidbody component on the player to control physics (serialized) </summary>
    private Rigidbody player;
    private bool isDead = false;
    private PlayersCollision playerCollision;
    //lewisSkinMesh
    [SerializeField]
    [Tooltip("Lewis prefab GameObject for access to the mesh")]
    private GameObject lewis;

    public PlayerManager playerManager;
    private PhotonView photonView = null;
    private string yAxisStr;

    private int countForFollowMethod = 0;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerManager = GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            otherPlayer = playerManager.OtherPlayer;
        }
        player = GetComponent<Rigidbody>();
        if (player == null)
        {
            Debug.LogWarning("Missing Rigidbody on player");
        }
        this.playerCollision = GetComponent<PlayersCollision>();

        // Get player id to move player in death mechanic
        if (playerManager.playerId == 1)
        {
            yAxisStr = "Vertical1";
        }
        else if (playerManager.playerId == 2)
        {
            yAxisStr = "Vertical2";
        }
    }

    /// <summary>
    /// Increments the count for hovering in the ReSpawnBubbleFollow() method
    /// </summary>
    private void FixedUpdate()
    {
        countForFollowMethod++;
    }

    /// <summary>
    /// This method moves the player back to the starting position
    /// and resets their movement if they fall below fallHeight
    /// </summary>
    /// 
    /// 
    /// <returns> Returns void </returns>
    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Missing Rigid Body");
            return;
        }
        
         if (this.isDead)
        {
            if (this.playerCollision.isSomethingInBeta() == true) {
                Revive();
            }
            ReSpawnBubbleFollow();
            //Debug.Log(player.transform.position.x);
            return;
        }

        this.isDead = player.transform.position.y < fallHeight;
        // Debug.Log(this.isDead);
        // Check if the player has fallen and will die
        Kill(this.isDead) ;

    }

    public bool IsDead()
    {
        return this.isDead;
    }

    /// <summary>
    /// Sets isDead to true and repositions the player to respawn location.
    /// Also turns off gravity
    /// </summary>
    /// <param name="willDie"></param>
    public void Kill(bool willDie)
    {
        if (otherPlayer == null)
        {
            otherPlayer = playerManager.OtherPlayer;
        }

        // If both dead replace both
        if (otherPlayer.GetComponent<Respawn>().IsDead() && this.isDead)
        {
            Respawn otherPlayerRespawn = otherPlayer.GetComponent<Respawn>();
            otherPlayerRespawn.Revive();
            otherPlayer.transform.position = Vector3.zero;

            this.Revive();
            this.player.transform.position = Vector3.right * 3f;
            return;
        }

        if (this.isDead)
        {
            foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.enabled = false;
            }
            this.GetComponent<MeshRenderer>().enabled = true;
            // Move the player and reset their movement

            if (PhotonNetwork.IsConnected && !photonView.IsMine)
            {
                return;
            }
            player.transform.position = Vector3.up * 2.4f + Vector3.right*player.transform.position.x;
            player.useGravity = false;
            player.velocity = Vector3.zero;

            // Turn off beta-alpha plane collisions
            playerCollision.SetCollisionRadius(0f);

            if (otherPlayer == null)
            {
                otherPlayer = this.playerManager.OtherPlayer;
            }

            otherPlayer.GetComponent<PlayersCollision>().SetCollisionRadius(0f);
        }
    }

    /// <summary>
    /// Makes dead player seed follow other player slowly and sinusoidally
    /// </summary>
    private void ReSpawnBubbleFollow()
    {
        playerManager.playerMovement.CheckIfGrounded();
        // Clamp the speed to the max allowed for the dead player
        if (player.velocity.x > maxVelocity_dead)
        {
            player.velocity = new Vector3(maxVelocity_dead, player.velocity.y, player.velocity.z);
        }

        // Make player hover slowly towards other player
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }
        player.transform.position = Vector3.right * Mathf.Lerp(
            player.position.x,
            otherPlayer.transform.position.x,
            maxVelocity_dead / (Mathf.Abs((player.position - otherPlayer.transform.position).magnitude)
            ))
            + Vector3.up * (0.7f * Mathf.Sin(countForFollowMethod * occilationFreq * maxVelocity_dead / 10f) + 2.1f);

        player.transform.position +=  Input.GetAxis(yAxisStr) * Vector3.up ;
    }

    private void Revive()
    {
        this.isDead = false;
        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = true;
        }
        this.GetComponent<MeshRenderer>().enabled = false;
        //Debug.Log("Players have collided");
        player.useGravity = true;

        // Turn on beta-alpha plane collisions
        playerCollision.SetCollisionRadius(2f);

        if (otherPlayer == null)
        {
            otherPlayer = this.playerManager.OtherPlayer;
        }

        otherPlayer.GetComponent<PlayersCollision>().SetCollisionRadius(2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision");
        if (playerManager == null)
        {
            playerManager = GetComponent<PlayerManager>();
        }

        // If you didn't collide with the co-op player but collided with something else, return out of function
        PlayerManager otherPlayerManager = collision.gameObject.GetComponent<PlayerManager>();
        if (otherPlayerManager == null) return;

        // If it was was a player (otherPlayer) then revive the player
        if (otherPlayerManager.playerId != playerManager.playerId)
        {
            Revive();
        }
        else
        {
            Debug.LogWarning("Not expected: report bug of Self collision!");
        }
    }
}
