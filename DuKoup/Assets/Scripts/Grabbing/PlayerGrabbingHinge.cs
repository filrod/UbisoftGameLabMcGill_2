using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerGrabbingHinge : MonoBehaviour
{
    /// <summary>
    /// The transform of the child component to place grabbed object
    /// </summary>
    [SerializeField][HideInInspector]
    private Vector3 grabbedPosition;

    /// <summary>
    /// Current object being grabbed
    /// </summary>
    private GameObject grabbable = null;

    //private GameObject grabPositionObj;
    private PlayerManager PM;
    private string key;
    private string hor;
    private string ver;
    private string throwkey;

    private float rbMass;

    private PlayerManager otherPM;

    private bool isGrabbing;
    private static bool playerIsGrabbed = false;
    private bool otherIsGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        PM = GetComponent<PlayerManager>();
        isGrabbing = false;

        // Get key code
        if (PM.playerId == 1)
        {
            key = "Grab1"; 
            throwkey = "Throw1";
            hor = "ThrowHor1";
            ver = "ThrowVer1";
        }
        else
        {
            throwkey = "Throw2";
            key = "Grab2";
            hor = "ThrowHor2";
            ver = "ThrowVer2";
        }
    }

    // Update is called once per frame
    void Update()
    {

        try
        {
            if ( !otherPM.isGrabbed && PM.isGrabbing )
            {
                UnGrab(GetGrabbedObject());
                
                playerIsGrabbed = false;
                PM.isGrabbing = false;
            }
        }
        catch (NullReferenceException e)
        {}

        try 
        {
            if ( isGrabbing && PM.CanThrow() && Input.GetButtonDown(throwkey))
            {
                UnGrab(GetGrabbedObject());
                Throw();

                if (PM.isGrabbing)
                {
                    playerIsGrabbed = false;
                    otherPM.isGrabbed = false;
                    PM.isGrabbing = false;
                }
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Should catch an object first");
        }

        if (isGrabbing && !isHolding())
        {
            UnGrab(GetGrabbedObject());

            if (PM.isGrabbing)
            {
                playerIsGrabbed = false;
                otherPM.isGrabbed = false;
                PM.isGrabbing = false;
            }

        }

        /// Doesn't work

    }

    private void Grab(GameObject grabbable)
    {
        
        this.grabbable = grabbable;
        grabbable.transform.position = this.transform.position + new Vector3(0f, 2.5f, 0f);

        Rigidbody rb = grabbable.GetComponent<Rigidbody>();
        rbMass = rb.mass;
        rb.mass = 0.01f;
    
        // Add Hinge Joint on the player with correct settings
        HingeJoint joint = gameObject.AddComponent<HingeJoint>();
        joint.axis = Vector3.back; /// (0,0,-1)
        joint.anchor = Vector3.zero;  // this.transform.position.y*Vector3.up;
        joint.enableCollision = true;

        HingeJoint jointForGrabbable = grabbable.AddComponent<HingeJoint>();
        jointForGrabbable.axis = Vector3.back;
        jointForGrabbable.anchor = Vector3.zero; // -Vector3.up*5f;
        jointForGrabbable.useSpring = true;
        jointForGrabbable.enableCollision = true;

        JointSpring hingeSpring = jointForGrabbable.spring; 
        hingeSpring.damper = 50;

        joint.connectedBody = grabbable.GetComponent<Rigidbody>();
        jointForGrabbable.connectedBody = this.GetComponent<Rigidbody>();
    }

    public GameObject GetGrabbedObject()
    {
        if (isGrabbing)
        {
            // Log an error if grabbed object is null but isGrabbing is true
            if (this.grabbable == null) {
                Debug.LogError("Object being grabbed is null despite isGrabbing being true. Fix playerGrabbingHinge.cs");
                return null;
            }
            return this.grabbable;
        }
        else
            return null;
    }

    private void UnGrab(GameObject grabbable)
    {
        isGrabbing = false;

        GameObject grabbed = this.grabbable;
        grabbed.transform.position = new Vector3(grabbed.transform.position.x, grabbed.transform.position.y, 0f);

        Rigidbody rb = grabbed.GetComponent<Rigidbody>();
        rb.mass = rbMass;

        Destroy(grabbable.GetComponent<HingeJoint>());
        Destroy(this.GetComponent<HingeJoint>());

        grabbable.GetComponent<Rigidbody>().useGravity = true;
        grabbable.GetComponent<Rigidbody>().AddForce(-0.23f * Physics.gravity * grabbable.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {

        if ( !isGrabbing && !playerIsGrabbed && other.gameObject.CompareTag("Grabbable") && Input.GetButtonDown(key) )
        {
            isGrabbing = true;
            other.transform.rotation = Quaternion.identity;
            Grab(other.gameObject);
        }

        if ( !isGrabbing && !playerIsGrabbed && other.gameObject.CompareTag("Player") && Input.GetButtonDown(key))
        {
            playerIsGrabbed = true;
            PM.isGrabbing = true;
            isGrabbing = true;

            otherPM = other.GetComponent<PlayerManager>();
            otherPM.isGrabbed = true;

            Grab(other.gameObject);
            
            if (transform.position.z != 0)
            {
                transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CapsuleCollider cap = this.GetComponent<CapsuleCollider>();
            cap.material.bounciness = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CapsuleCollider cap = this.GetComponent<CapsuleCollider>();
            cap.material.bounciness = 1;
        }
    }

    private bool isHolding()
    {
        return Input.GetButton(key);
    }

    private void Throw(){

        Vector3 dir;
        //grabbable.transform.position += new Vector3 (0, 0, 0.3f);
        // grabbable.get

        // if ( Input.GetAxis(hor) == 0  && Input.GetAxis(hor) == 0)
        //     dir = Vector3.up;
        // else
            dir = new Vector3(Input.GetAxis(hor) , Input.GetAxis(ver), 0).normalized;

        int force = 15 ;
        grabbable.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
    }
}
