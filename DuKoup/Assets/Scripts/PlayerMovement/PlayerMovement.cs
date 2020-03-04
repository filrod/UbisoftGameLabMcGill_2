﻿using System;
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
    private PlayerManager playerManager;

    [SerializeField] Animator animator;


    /// <summary> Player identification for distiction between player 1 and 2 (serialized) </summary>
    [SerializeField] [Tooltip("A number, either 1 or 2, to say which player this is. This is used for player input managment")]
    private int playerId;

    /// <summary>
    /// Ability to high jump
    /// </summary>
    [SerializeField][Tooltip("Player has ability to high jump. In inspector for testing purposes only.")]
    private bool canDoubleJump = false;

    public bool CanDoubleJump
    {
        get
        {
            return canDoubleJump;
        }
        set
        {
            canDoubleJump = value;
        }
    }

    /// <summary> A rigidbody component on the player to control physics (serialized) </summary>
    [Tooltip("A rigidbody component on the player to control physics")]
    [SerializeField] private Rigidbody player;

    /// <summary> Speed parameter for horizontal movement (serialized) </summary>
    [Tooltip("Speed parameter for horizontal movement")]
    [SerializeField] [Range(0f, 25f)] private float horizontalSpeed = 10f;

    /// <summary> Speed parameter for horizontal movement (serialized) </summary>
    [Tooltip("Speed parameter for horizontal movement while in the air. Should be slower than while not airborne")]
    [SerializeField] [Range(0f, 20f)] private float horizontalSpeedInJump = 9f;

    /// <summary>
    /// A boolean to check if the player is grounded.
    /// Gets set in CheckIfGrounded() (called in Update())
    /// via comparison between a raycast hit distance straight
    /// down and the player gameobject's collider.bound.extent.y
    /// distance
    /// </summary>
    private bool grounded = true;
    /// <summary>
    /// Sets the Jumping force
    /// </summary>
    [SerializeField] [Range(0f, 10f)] [Tooltip("Sets the Jumping Force")]
    private float jumpForce = 6;
    public float JumpForce
    {
        get
        {
            return jumpForce;
        }
        set
        {
            jumpForce = value;
        }
    }


    /// <summary>
    /// Number of current jumps done before hitting the ground (which sets this to zero again)
    /// </summary>
    private int nbJumps = 0;
    /// <summary>
    /// Maximum jumps allowed before player hits the ground again
    /// </summary>
    private int maxJumps = 2;
    /// <summary>
    /// Boolean to know when player has pressed 
    /// jump. Avoids overhead by allowing the check 
    /// to happen in Update() and not FixedUpdate().
    /// </summary>
    private bool jump = false;

    /// <summary>
    /// A butt slam force multiplier that scales by a squared factor
    /// </summary>
    [SerializeField] [Range(1f, 6.0f)] [Tooltip("Changes force multiplier of butt slam by this factor squared")]
    private float buttForce = 5;


    /// <summary> Movement smoothing parameter for crossing between playable planes (serialized) </summary>
    [Tooltip("Movement smoothing parameter for crossing between playable planes")]
    [Range(0.0f, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    /// <summary> A constant that makes gravity more intense at the peak of one's jump (only for high jumps). </summary>
    [Tooltip("A constant that makes gravity more intense at the peak of one's jump (only for high jumps).")]
    [Range(0.0f, 8f)] [SerializeField] private float gravityMultiplier = 4f;

    public float GravityMultiplier
    {
        get
        {
            return gravityMultiplier;
        }
        set
        {
            gravityMultiplier = value;
        }
    }


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
    [SerializeField][HideInInspector]
    [Tooltip("The collider height from halfway down. " +"This is what helps check if the player is grunded since the rays " + "to check get cast from the midle of the game object downward.")]
    private float playerHeightWaistDown = 1.26f;

    // Keep track on where the player is facing
    private bool m_FacingRight = true;

    [SerializeField] private Collider2D confinedArea;


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
    public void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        SetPlayerHeightFromCollider( player.GetComponent<Collider>() );
        movementXY = new Vector2(0, 0);

        if (playerId == 1)
        {
            horizontalAxis = "Horizontal1";
            jumpButton = "Jump1";
        }
        else if (playerId == 2)
        {
            horizontalAxis = "Horizontal2";
            jumpButton = "Jump2";
        }
    }

    public void Update()
    {
        CheckIfGrounded();
        // Set boolean to true if jump is pressed
        this.jump = Input.GetButtonDown(jumpButton);

        if (jump)
        {
            Jump();
        }

        restrictObject(confinedArea);

        // Restart game by pressing F4
        if (Input.GetKeyDown(KeyCode.F4)){
            Application.LoadLevel(0);
        }

        // Play walkin animation if player has high enough velocity
        bool isMoving = player.velocity.magnitude > 0.01;
        animator.SetBool("isWalking", isMoving);
    }

    public bool CheckIfGrounded()
    {
        // Check if grounded 
        RaycastHit groundCollisionInfo;
        Physics.Raycast(player.transform.position, -Vector3.up, out groundCollisionInfo, 20f);
        float distToGround = player.transform.position.y - groundCollisionInfo.point.y;
        // Debug.Log("Dist to ground" + distToGround);
        //this.distToGround = groundCollisionInfo;

        bool tempGrounded = (distToGround <= playerHeightWaistDown);

        if (!this.grounded && tempGrounded)
        {
            animator.SetBool("isJumping", false); 
        }

        this.grounded = tempGrounded;
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

        if (!this.grounded)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                ButtSlam();
            }
            else
            {
                SkewJumpParabola();
            }

            movementXY.x = Input.GetAxis(horizontalAxis) * horizontalSpeedInJump;
            movementXY.y = 0;
        }
        else
        {
            movementXY.x = Input.GetAxis(horizontalAxis) * horizontalSpeed;
            movementXY.y = 0;
        }

        // Flip the player

        // If the input is moving the player right and the player is facing left...
        if (movementXY.x > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (movementXY.x < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(movementXY.x, player.velocity.y);
        // And then smoothing it out and applying it to the character
        targetVelocity = Vector3.SmoothDamp(player.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        float distance = new Vector3(targetVelocity.x, targetVelocity.y, 0).magnitude * Time.fixedDeltaTime; // Distance from player to where player will be next frame
        movementXY.Normalize(); // Normalize movementXY since it should be used to indicate direction
        //RaycastHit hit;

        // Check if the player is not on the ground and that the current velocity will result in a collision
        // if (!grounded && player.SweepTest(movementXY, out hit, distance))
        // {
        //     // Stopping the horizontal movement of the player
        //     player.velocity = new Vector3(0, player.velocity.y, 0);
        // }
        // else
        // {
            // If not jumping or no collision proceed as normal
        player.velocity = targetVelocity;
        
        // }


    }

    /// <summary>
    /// Getter method for playerID
    /// </summary>
    /// <returns> Returns an integer 1 or 2 depending on the player using this class </returns>
    public int GetPlayerId()
    {
        return this.playerId;
    }

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

        //Debug.Log("isGrounded" + this.grounded);
        //Debug.Log("Player height: " + this.playerHeightWaistDown);
        if (this.grounded)
        {
            nbJumps = 0;
        }
        if (this.grounded || (nbJumps < maxJumps) && this.canDoubleJump)
        {
            animator.SetBool("isJumping", true); // Play jumping animation
            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
            player.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            nbJumps += 1;
            grounded = false;
        }
        
        this.jump = false;
    }

    /// <summary>
    /// Intensifies gravity at the end of a jump to get a videogame feel
    /// </summary>
    private void SkewJumpParabola()
    {
        // Check if player has passed peak of jump
        if (this.player.velocity.y < 0)
        {
            //Debug.Log("SkewJump");
            player.AddForce( Vector3.up * Physics.gravity.y * this.gravityMultiplier );
        }
    }

    public void ButtSlam()
    {
        
        // Check if player has passed peak of jump
        if (Input.GetKeyDown(KeyCode.B) && !this.grounded)
        {
            Debug.Log("Butt slam!!!" + "ground: " + grounded);
            player.AddForce(Vector3.up * Physics.gravity.y * Mathf.Pow(this.buttForce, 2));
        }
    }

    /// <summary>
    /// Sets the player's height to the extent (centre to tip) of the collider in the y-axis.
    /// </summary>
    /// <param name="colliderAttachedToPlayer"></param>
    private void SetPlayerHeightFromCollider(Collider colliderAttachedToPlayer)
    {
        float epsillon = 0.05f;
        this.playerHeightWaistDown = colliderAttachedToPlayer.bounds.extents.y + epsillon;
        //debug.Log("Player height: " + this.playerHeightWaistDown);
    }


    /// <summary>
    /// @Robin
    /// Flip the player when changing directions.
    /// </summary>
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1f;
        transform.localScale = theScale;
    }

    private void restrictObject(Collider2D area)
    {                 
        // get the current position
        Vector3 clampedPosition = transform.position;
        // limit the x and y positions to be between the area's min and max x and y.
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, area.bounds.min.x, area.bounds.max.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, area.bounds.min.y, area.bounds.max.y);
        // z remains unchanged
        // apply the clamped position
        transform.position = clampedPosition;
    }

    /// <summary>
    /// Reset all the parameter to default
    /// </summary>
    public void reset()
    {
        canDoubleJump = false;
        jumpForce = 6.0f;
        gravityMultiplier = 1.3f;
    }
} 

