using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Filipe Rodrigues
 * @Date 2020/01/28
 * 
 * This class controls player movement
*/

public class PlayerMovement : MonoBehaviour
{
    // Fields 

    [SerializeField] private int playerId;
    [SerializeField] private Rigidbody player;
    [SerializeField] private float speed = 10f;

    [Range(0.0f, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [Range(5.0f, 15.0f)] [SerializeField] private float jumpVelocity = 7.0f;
    [Range(0.0f, 6.0f)] [SerializeField] private float gravityMultiplier = 4.5f;
    [Range(0.0f, 6.0f)] [SerializeField] private float lowJumpGravityMultiplier = 2f;


    private Vector2 movementXY;
    private string horizontalAxis;
    private string jumpButton;
    private Vector3 m_Velocity = Vector3.zero;

    private const float g = 9.81f;
    private const float averageHumanJump = 2.5f; // Times your mass on earth


    /**
     * 
     * This method distinguishes which objet is using this 
     * class and assigns an axis configuration based on the
     * playerId
     * 
     * Initiates horizontalMovement to be zero for use in 
     * FixedUpdate()
     */
    public void Start()
    {
        movementXY = new Vector2(0, 0);

        if (playerId == 1)
        {
            horizontalAxis = "Horizontal1";
            jumpButton = "Vertical1";
        }
        else if (playerId == 2)
        {
            horizontalAxis = "Horizontal2";
            jumpButton = "Vertical2";
        }
    }

    /**
     * FixedUpdate() gets called twice per frame (around 50 
     * times per second) and is best used to compute values 
     * for physics.
     * 
     * 
     */
    public void FixedUpdate()
    {
        movementXY.x = Input.GetAxis(horizontalAxis) * speed;
        movementXY.y = 0;
        if (Input.GetButtonDown(jumpButton))
        {
            
            //player.AddForce(transform.up * player.mass * g * averageHumanJump*15f*Time.deltaTime, ForceMode.Impulse);
            player.velocity += Vector3.up * jumpVelocity;

            // This next part makes jumps more videogame-like

            // If player is falling back down
            if (player.velocity.y < 0)
            {
                player.velocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;
                //Debug.Log("Falling");
            }
            // If player is going up in the jump and not still holding jump button down
            else if (player.velocity.y > 0 && !Input.GetButton(jumpButton))
            {
                player.velocity += Vector3.up * Physics.gravity.y * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
                //Debug.Log("Low jump");
            }
        }


        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(movementXY.x, player.velocity.y);
        // And then smoothing it out and applying it to the character
        player.velocity = Vector3.SmoothDamp(player.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    public int getPlayerId()
    {
        return this.playerId;
    }

} 
