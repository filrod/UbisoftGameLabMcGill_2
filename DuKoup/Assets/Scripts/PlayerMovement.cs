using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Filipe Rodrigues, Robin Leman
 * @Date 2020/01/28
 * 
 * This class controls player movement
*/

public class PlayerMovement : MonoBehaviourPun
{
    // Fields 

    /// <summary> Player identification for distiction between player 1 and 2 (serialized) </summary>
    // [SerializeField] private int playerId;
    // Used PhotonNetwork.LocalPlayer.ActorNumber instead

    /// <summary> A rigidbody component on the player to control physics (serialized) </summary>
    [Tooltip("A rigidbody component on the player to control physics")]
    [SerializeField] private Rigidbody player;

    /// <summary> Speed parameter for horizontal movement (serialized) </summary>
    [Tooltip("Speed parameter for horizontal movement")]
    [SerializeField] private float speed = 10f;

    /// <summary> Movement smoothing parameter for crossing between playable planes (serialized) </summary>
    [Tooltip("Movement smoothing parameter for crossing between playable planes")]
    [Range(0.0f, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    /// <summary> Jump velocity controls how fast player leaves the ground which affets jump height (serialized) </summary>
    [Tooltip("Jump velocity controls how fast player leaves the ground which affets jump height")]
    [Range(5.0f, 15.0f)] [SerializeField] private float jumpVelocity = 7.0f;

    /// <summary> A constant that makes gravity more intense at the peak of one's jump (only for high jumps). </summary>
    [Tooltip("A constant that makes gravity more intense at the peak of one's jump (only for high jumps).")]
    [Range(0.0f, 6.0f)] [SerializeField] private float gravityMultiplier = 2f;

    /// <summary> A constant that makes gravity more intense at the peak of one's small hop. </summary>
    [Tooltip("A constant that makes gravity more intense at the peak of one's small hop.")]
    [Range(0.0f, 6.0f)] [SerializeField] private float lowJumpGravityMultiplier = 5f;

    [Tooltip("Planking cript behaiour reference")] [SerializeField] private PlankingBehaviour plankingBehaviour;

    /// <summary> 2D Vector for horizontal and vertical movement respectively </summary>
    private Vector2 movementXY;
    /// <summary> Horizontal axis string to store which player's horizontal axis to access. </summary>
    private string horizontalAxis;
    /// <summary> Jump button string to be mapped in the input manager in project settings. </summary>
    private string jumpButton;
    /// <summary> Velocity used in smoothing </summary>
    private Vector3 m_Velocity = Vector3.zero;

    private const float g = 9.81f;
    private const float averageHumanJump = 2.5f; // Times your mass on earth



    /// <summary>
    /// This method distinguishes which objet is using this 
    /// class and assigns an axis configuration based on the
    /// playerId
    /// 
    /// Initiates horizontalMovement to be zero for use in 
    /// FixedUpdate()
    /// </summary>
    /// 
    /// 
    /// <returns> Returns void </returns>
    public void Start()
    {
        movementXY = new Vector2(0, 0);

        horizontalAxis = "Horizontal1";
        jumpButton = "Vertical1";
    }

    /// <summary>
    /// FixedUpdate() gets called twice per frame(around 50 
    /// times per second) and is best used to compute values
    /// for physics.
    /// 
    /// Sets Horizontal movement and vertical movement with jump() method.
    /// Calls:
    /// jump()
    /// </summary>
    public void FixedUpdate()
    {
        // Avoid movement for planking player
        // if ( GetPlayerId()==2 && plankingBehaviour.PlayerIsPlanking() ) return;
        if (base.photonView.IsMine)
        {
            Debug.Log("Move");
            movementXY.x = Input.GetAxis(horizontalAxis) * speed;
            movementXY.y = 0;

            Jump();
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(movementXY.x, player.velocity.y);
            // And then smoothing it out and applying it to the character
            player.velocity = Vector3.SmoothDamp(player.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
    }

    /// <summary>
    /// Getter method for playerID
    /// </summary>
    /// <returns> Returns an integer 1 or 2 depending on the player using this class </returns>
    public int GetPlayerId()
    {
        return PhotonNetwork.LocalPlayer.ActorNumber;
    }

    /// <summary>
    /// Method to jump. Contains modifier for both a short hop and a longer/higher jump.
    /// Modifies gravity to make descent faster than ascent.
    /// </summary>
    /// <returns> Returns Void </returns>
    private void Jump()
    {
        // Add check to see if player is touching the ground
        if (Input.GetButtonDown(jumpButton) && player.velocity.y==0)
        {

            //player.AddForce(transform.up * player.mass * g * averageHumanJump*15f*Time.deltaTime, ForceMode.Impulse);
            player.velocity += Vector3.up * jumpVelocity;

        }


        // This next part makes jumps more videogame-like

        // If player is falling back down
        if (player.velocity.y < 0)
        {
            player.velocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;
            Debug.Log("Falling");
        }
        // If player is going up in the jump and not still holding jump button down
        else if (player.velocity.y > 0 && !Input.GetButton(jumpButton))
        {
            player.velocity += Vector3.up * Physics.gravity.y * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
            Debug.Log("Low jump");
        }
        
    }
} 
