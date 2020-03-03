using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Name: PlayerSwinging
/// 
/// Description:
/// Controls the swinging mechanic of the player.
/// To be attached on dummy
/// </summary>

public class PlayerSwinging : MonoBehaviour
{

    [SerializeField] [Tooltip("End of the rope object, where the player grabs the swinging rope")] private GameObject rope;
    private bool isSwinging = false;


    /// <summary>
    /// Method Name: exampleMethod()
    /// 
    /// Description:
    /// Detects collision with the end of the rope
    /// 
    void OnTriggerEnter(Collider obj)
    {
        // Add Input.GetKeyDown() if you want to add a control.
        if (obj.gameObject.CompareTag("SwingingRope"))
        {
            Swing();
            isSwinging = true;
        }
    }

    void Swing()
    {

        // Add Hinge Joint on the player with correct settings
        HingeJoint joint = gameObject.AddComponent<HingeJoint>();
        joint.axis = Vector3.back; /// (0,0,-1)
        joint.anchor = Vector3.zero;

        joint.useSpring = true;
        JointSpring hingeSpring = joint.spring;
        hingeSpring.damper = 50;

        // Connect the player to the rope
        joint.connectedBody = rope.GetComponent<Rigidbody>();
    }

}
