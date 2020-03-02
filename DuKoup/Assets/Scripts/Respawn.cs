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

    /// <summary> Other player </summary>
    [Tooltip("The player to follow when you respawn")]
    [SerializeField] private GameObject otherPlayer;

    /// <summary> A rigidbody component on the player to control physics (serialized) </summary>
    private Rigidbody player;
    private bool isDead = false;
    private PlayersCollision playerCollision = new PlayersCollision();
    //lewisSkinMesh

    private void Start()
    {
        player = GetComponent<Rigidbody>();
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

        if (this.isDead)
        {
            ReSpawnBubbleFollow();
            Debug.Log(player.transform.position.x);
            return;
        }

        this.isDead = player.transform.position.y < fallHeight;
        Debug.Log(this.isDead);
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
        
        if (this.isDead)
        {
            //this.lewisSkinMesh.enabled = false;
            this.GetComponent<MeshRenderer>().enabled = true;
            // Move the player back to the origin and reset their movement
            player.transform.position = Vector3.up * 2.4f;
            player.useGravity = false;
            player.velocity = Vector3.zero;
            playerCollision.SetCollisionRadius(0f);
        }
    }

    /// <summary>
    /// Makes dead player seed follow other player slowly and sinusoidally
    /// </summary>
    private void ReSpawnBubbleFollow()
    {
        player.transform.position = Vector3.right * Mathf.Lerp(
            player.position.x,
            otherPlayer.transform.position.x,
            1f/(5f*Mathf.Abs((player.position - otherPlayer.transform.position).magnitude)
            ))
            + Vector3.up * ( 0.7f * Mathf.Sin(Time.frameCount/10f) + 2.1f);

    }

    private void Revive()
    {
        this.isDead = false;
        //this.GetComponentInChildren<MeshRenderer>().enabled = true;
        this.GetComponent<MeshRenderer>().enabled = false;
        Debug.Log("Players have collided");
        player.useGravity = true;
        playerCollision.SetCollisionRadius(2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision");
        if ((player.name == "dummy1" && collision.gameObject.name == "dummy2") || (player.name == "dummy2" && collision.gameObject.name == "dummy1"))
        {
            Revive();

        }
    }
}
