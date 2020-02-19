using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private PlayerManager playerManager;

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
        playerManager.GravityMultiplier *= 0.8f;
    }

    public void GainFastSpeed()
    {

    }

}
