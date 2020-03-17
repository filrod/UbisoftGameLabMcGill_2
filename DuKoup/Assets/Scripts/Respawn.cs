using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ubisoft Games Lab McGill
/// -----------------------------
/// @authors Filipe, Thomas
/// </summary>

public class Respawn : MonoBehaviour
{
    // Fields
    /// <summary> Fall height </summary>
    [Tooltip("The height below the level that the player should reset at if they fall below")]
    [SerializeField] private float fallHeight = -14.0f;

    /// <summary>
    /// Enum to get and set fallHeight
    /// </summary>
    public float FallHeight {
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
    [Tooltip("The player to follow when you respawn")] [HideInInspector]
    [SerializeField] private GameObject otherPlayer;

    /// <summary>
    /// The max velocity the player can move while dead
    /// </summary>
    [SerializeField]
    [Tooltip("Player movenment max velocity when dead")]
    [Range(0f, 0.5f)]
    private float maxVelocity_dead = 0.05f;

    /// <summary>
    /// Controls upward movement while dead. Helps simulate an analogue stick for digital input.
    /// </summary>
    private float playerVerticalBubbleMovement = 0f;

    [SerializeField]
    [Tooltip("Increase parameter for player's vertical velocity when in the death bubble and pressing up")]
    [Range(0f, 0.8f)]
    private float dy = 0.035f;

    /// <summary>
    /// The value the dead player can move above and bellow the y value of the other live player
    /// </summary>
    [SerializeField]
    [Tooltip("Range of possible vertical movement away from other player who is alive")]
    [Range(0f, 20f)]
    private float yRangedeadPlayer = 8f;
    public float YRangedeadPlayer
    {
        get { return yRangedeadPlayer; }
        set { yRangedeadPlayer = value; }
    }

    /// <summary>
    /// The middle of the ossilations in y for the dead player. Measured from other player's y coordinate. Suggested to set this to the other player's top of collider
    /// </summary>
    [SerializeField]
    [Tooltip("The middle of the ossilations in y for the dead player. Measured from other player's y coordinate. Suggested to set this to the other player's top of collider")]
    [Range(-5.5f, 5.5f)]
    private float deltaYHoverFromOtherPlayer = 2.1f;
    public float DeltaYHoverFromOtherPlayer
    {
        get { return deltaYHoverFromOtherPlayer; }
        set { deltaYHoverFromOtherPlayer = value; }
    }

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

    [HideInInspector]
    public PlayerManager playerManager;
    private PhotonView photonView = null;
    private string yAxisStr;

    private int countForFollowMethod = 0;

    [SerializeField] private AudioClip bubblePop1;
    [SerializeField] private AudioClip bubblePop2;
    private AudioSource audio;

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
        audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Increments the count for hovering in the ReSpawnBubbleFollow() method
    /// </summary>
    private void FixedUpdate()
    {
        countForFollowMethod++;

        SetBubbleYMovement();
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
            //if (this.playerCollision.isSomethingInBeta() == true)
            //{
            //    Revive();
            //}
            // ^ that creates a revive bug in no passing zones
            // the fix:
            Vector2 dist2D = this.player.transform.position - this.otherPlayer.transform.position;
            Vector2 extentsOfCollider = this.player.GetComponent<CapsuleCollider>().bounds.extents;
            if (dist2D.magnitude <= 1.35f*extentsOfCollider.magnitude) Revive();

            ReSpawnBubbleFollow();
            //Debug.Log(player.transform.position.x);
            return;
        }

        this.isDead = player.transform.position.y < fallHeight;
        // Debug.Log(this.isDead);
        // Check if the player has fallen and will die
        Kill(this.isDead);

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
        if (otherPlayer.GetComponent<Respawn>().IsDead() && willDie)
        {
            /*Respawn otherPlayerRespawn = otherPlayer.GetComponent<Respawn>();
            otherPlayerRespawn.Revive();
            otherPlayer.transform.position = Vector3.zero;

            this.Revive();
            this.player.transform.position = Vector3.right * 3f;
            return;*/
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (willDie)
        {
            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                skinnedMeshRenderer.enabled = false;
            }
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
            player.transform.position = Vector3.up * (otherPlayer.transform.position.y + this.deltaYHoverFromOtherPlayer) + Vector3.right*player.transform.position.x;
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
    /// Sets the y range a player can move when dead, 
    /// sets the playerVerticalBubbleMovement 
    /// and performs a lerp towards the other player 
    /// in the y axis when user does nothing
    /// </summary>
    private void SetBubbleYMovement()
    {
        bool bellowUpperBound = this.player.transform.position.y < this.otherPlayer.transform.position.y + yRangedeadPlayer;
        bool aboveLowerBound = this.player.transform.position.y > this.otherPlayer.transform.position.y - yRangedeadPlayer;
        bool inRange = bellowUpperBound && aboveLowerBound;

        if (!bellowUpperBound)
        {
            /// Slowly move towards the other player (only in the y component here)
            this.playerVerticalBubbleMovement = Mathf.Lerp(this.playerVerticalBubbleMovement, otherPlayer.transform.position.y, maxVelocity_dead / (Mathf.Abs((player.position - otherPlayer.transform.position).magnitude)));
        }
        else if (inRange)
        {
            /// If user presses to move up increment the player movement for smooth increase to happen in ReSpawnBubbleFollow
            /// if user presses to move down decrement for the same negative effect
            /// else, move slowly towards other alive player (only in the y component here)
            if (Input.GetAxis(yAxisStr) > 0)
                this.playerVerticalBubbleMovement += dy;
            else if (Input.GetAxis(yAxisStr) < 0)
                this.playerVerticalBubbleMovement -= dy;
            else
                this.playerVerticalBubbleMovement = Mathf.Lerp(this.playerVerticalBubbleMovement, otherPlayer.transform.position.y, maxVelocity_dead / (Mathf.Abs((player.position - otherPlayer.transform.position).magnitude)));
        }
        else if (!aboveLowerBound)
        {
            /// Slowly move towards the other player (only in the y component here)
            this.playerVerticalBubbleMovement = Mathf.Lerp(this.playerVerticalBubbleMovement, otherPlayer.transform.position.y, maxVelocity_dead / (Mathf.Abs((player.position - otherPlayer.transform.position).magnitude)));
        }
        else
        {
            /// Just in case the logic has faults
            Debug.LogWarning("Look at Respawn.FixedUpdate. Unexpected outcome of if statements.");
        }
    }

    /// <summary>
    /// Makes dead player seed follow other player slowly and sinusoidally
    /// </summary>
    private void ReSpawnBubbleFollow()
    {
        if (audio.isPlaying)
            audio.Stop();
        //playerManager.playerMovement.CheckIfGrounded();  // Not sure this is needed since inheriting the code from Thomas
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
        //if (player.transform.position.y <= this.fallHeight)
        //    player.transform.position = new Vector3(player.transform.position.x, otherPlayer.transform.position.y, player.transform.position.z);

        Vector3 lerpedXposition = Vector3.right * Mathf.Lerp(
            player.position.x,
            otherPlayer.transform.position.x,
            maxVelocity_dead / (  Mathf.Abs( (player.position - otherPlayer.transform.position).magnitude )  )
            );
        /// This one is not lerped since lerping happens in SetBubbleYMovement() called in FixedUpdate due to framerate dependancy
        /// lerping only happens there if the user is not sending any inputs in y as to not fight the lerp and allow more freedom
        Vector3 sinWaveNonLerpY = Vector3.up * (
            this.playerVerticalBubbleMovement 
            + this.deltaYHoverFromOtherPlayer + otherPlayer.transform.position.y 
            + 0.7f * Mathf.Sin(countForFollowMethod * occilationFreq * maxVelocity_dead / 10f)
            );

        /// Do the rayman bubble follow thing
        player.transform.position = lerpedXposition + sinWaveNonLerpY;
    }

    /// <summary>
    /// Revives the player. In order:
    /// - renders all meshes and skinnedMeshes the player has
    /// - hides the bubble texture
    /// - makes player use gravity
    /// - sets player radius for alpha-beta plane collisions to 2f
    /// 
    /// </summary>
    private void Revive()
    {
        if (this.isDead)
        {
            if (audio.isPlaying)
                audio.Stop();
            
            audio.PlayOneShot(bubblePop1, 1);
        }
        this.isDead = false;
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            skinnedMeshRenderer.enabled = true;
        }
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

    /// <summary>
    /// Checks for collisions between players and revives if need be
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision");
        if (playerManager == null)
        {
            playerManager = GetComponent<PlayerManager>();
        }

        // If you didn't collide with the co-op player but collided with something else, return out of function
        PlayerManager otherPlayerManager = collision.gameObject.GetComponent<PlayerManager>();
        bool hitAPlayer = collision.gameObject.CompareTag("Player");
        if (hitAPlayer)
        {
            Revive();
            Debug.Log("Revived by" + collision.gameObject.name);
        }
        //else
            // Uncomment to find weird misplaced colliders
            //Debug.LogWarning("Ignore: "+collision.gameObject.name);
    }
}
