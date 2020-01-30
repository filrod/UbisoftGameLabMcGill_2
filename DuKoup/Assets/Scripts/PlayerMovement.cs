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
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    private float horizontalMovement;
    private Vector3 movement;
    private string axis;
    private Vector3 m_Velocity = Vector3.zero;


    // Start is called before the first frame update
    public void Start()
    {
        horizontalMovement = 0;
        movement = new Vector3(horizontalMovement, 0f, 0f);

        if (playerId == 1)
            axis = "Horizontal1";
        else if (playerId == 2)
            axis = "Horizontal2";
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        horizontalMovement = Input.GetAxis(axis) * speed;

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(horizontalMovement, player.velocity.y);
        // And then smoothing it out and applying it to the character
        player.velocity = Vector3.SmoothDamp(player.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    public int getPlayerId()
    {
        return this.playerId;
    }
} 
