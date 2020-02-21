using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerMovement playerMovement = null;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// The player will get low gravity multipler to jump higher
    /// </summary>
    public void GainLowGravity()
    {
        Debug.Log("GainLowGravity");
        if (playerMovement == null)
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }
        playerMovement.GravityMultiplier *= 0.8f;
    }

    /// <summary>
    /// The player will get the ability to jump high
    /// </summary>
    public void GainHighJump()
    {
        Debug.Log("GainHighJump");
        if (playerMovement == null)
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }
        playerMovement.CanHighJump = true;
    }


    /// <summary>
    /// The player will get the ability to grab property
    /// </summary>
    public void GainGrabable()
    {
        Debug.Log("GainGrabable");
    }

    public void Reset()
    {
        Debug.Log("Reset");
        if (playerMovement == null)
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }
        playerMovement.reset();
    }
}
