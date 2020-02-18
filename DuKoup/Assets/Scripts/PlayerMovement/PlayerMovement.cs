using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UBISOFT GAMES LAB - McGill Team #2
/// -----------------------------------
/// @author Filipe Rodrigues, Robin Leman
/// @Date 2020/02/12
///
/// This class controls player movement
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    // Fields 

    /// <summary> Player identification for distiction between player 1 and 2 (serialized) </summary>
    [SerializeField] private int playerId;

    /// <summary>
    /// Ability to high jump
    /// </summary>
    [SerializeField] private bool canHighJump = false;

    /// <summary> A rigidbody component on the player to control physics (serialized) </summary>
    [Tooltip("A rigidbody component on the player to control physics")]
    [SerializeField] private Rigidbody player;

    /// <summary> Speed parameter for horizontal movement (serialized) </summary>
    [Tooltip("Speed parameter for horizontal movement")]
    [SerializeField] private float speed = 800.0f;

    private bool grounded = true;
    private float jumpForce = 300f;
    private int nbJumps = 0;
    private int maxJumps = 2;
    private bool jump = false;


    /// <summary> Movement smoothing parameter for crossing between playable planes (serialized) </summary>
    [Tooltip("Movement smoothing parameter for crossing between playable planes")]
    [Range(0.0f, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    /// <summary> Jump velocity controls how fast player leaves the ground which affets jump height (serialized) </summary>
    [Tooltip("Jump velocity controls how fast player leaves the ground which affets jump height")]
    [Range(5.0f, 15.0f)] [SerializeField] private float jumpVelocity = 7.0f;

    /// <summary> A constant that makes gravity more intense at the peak of one's jump (only for high jumps). </summary>
    [Tooltip("A constant that makes gravity more intense at the peak of one's jump (only for high jumps).")]
    [Range(0.0f, 6.0f)] [SerializeField] private float gravityMultiplier = 5f;

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

    /// <summary>
    /// Contains info on the raycast checking if the player 
    /// </summary>
    private RaycastHit distToGround;

    /// <summary>
    /// The collider height from halfway down. This is what helps 
    /// check if the player is grunded since the rays to check get 
    /// cast from the midle of the game object downward.
    /// </summary>
    [SerializeField]
    [Tooltip("The collider height from halfway down. " +
        "This is what helps check if the player is grunded since the rays " +
        "to check get cast from the midle of the game object downward."
        )]
    private float playerHeightWaistDown = 1.26f;

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

    public void Update()
    {
        CheckIfGrounded();
        // Set boolean to true if jump is pressed
        this.jump = Input.GetButtonDown(jumpButton);
    }

    public bool CheckIfGrounded()
    {
        // Check if grounded 
        RaycastHit groundCollisionInfo;
        Physics.Raycast(player.transform.position, -Vector3.up, out groundCollisionInfo, 20f);
        float distToGround = player.transform.position.y - groundCollisionInfo.point.y;
        //Debug.Log("Dist to ground" + distToGround);
        //this.distToGround = groundCollisionInfo;

        this.grounded = (distToGround <= playerHeightWaistDown);
        return this.grounded;
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
        //if ( GetPlayerId()==2 && plankingBehaviour.PlayerIsPlanking() ) return;

        movementXY.x = Input.GetAxis(horizontalAxis) * speed * Time.deltaTime;
        movementXY.y = 0;


        if (jump)
        {
            Jump();
        }
        
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(movementXY.x, player.velocity.y);
        // And then smoothing it out and applying it to the character
        player.velocity = Vector3.SmoothDamp(player.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    /// <summary>
    /// Getter method for playerID
    /// </summary>
    /// <returns> Returns an integer 1 or 2 depending on the player using this class </returns>
    public int GetPlayerId()
    {
        return this.playerId;
    }

    //public bool IsGrounded(){
    //    Physics.Raycast(transform.position, -Vector3.up, distToGround);
    //    return Mathf.Abs(distToGround.transform-transform.position)<0.1;
    //}

/// <summary>
/// Method to jump. Contains modifier for both a short hop and a longer/higher jump.
/// Modifies gravity to make descent faster than ascent.
/// 
/// 
/// Need to fix for time.deltatime
/// </summary>
/// <returns> Returns Void </returns>
private void Jump()
    {
        Debug.Log("isGrounded" + this.grounded);
        if (this.grounded)
        {
            nbJumps = 0;
        }
        if (this.grounded || (nbJumps < maxJumps) && this.canHighJump)
        {
            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
            player.AddForce(new Vector3(0, jumpForce, 0));
            nbJumps += 1;
            grounded = false;
        }
        this.jump = false;
        
    }
} 
