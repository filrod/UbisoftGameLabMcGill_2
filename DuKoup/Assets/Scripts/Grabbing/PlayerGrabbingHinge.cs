using UnityEngine;

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
    private PlayerManager playerManager;
    private KeyCode key;
    private bool isGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {

        playerManager = GetComponent<PlayerManager>();

        // Get key code
        if (playerManager.playerId == 1)
            key = KeyCode.Z; 
        else
            key = KeyCode.RightShift;

        //foreach (var child in this.GetComponentsInChildren<GameObject>())
        //{
        //    if (child.name == "Grabbed position")
        //    {
        //        defaultTrans = child.GetComponent<Transform>();
        //        grabPositionObj = child;
        //        return;
        //    }
        //}
        //if (defaultTrans == null)
        //{
        //    grabbedPosition = this.transform.position + new Vector3(0f, -0.1f, 2f);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        // If a grabbable has been set, update it's position
        if (isGrabbing)
            grabbable.transform.position = this.transform.position + new Vector3(0f, 3f, 0f);
    }

    private void Grab(GameObject grabbable)
    {
        this.grabbable = grabbable;
        grabbable.transform.position = this.transform.position + new Vector3(0f, 3f, 0f);
        isGrabbing = true;
        // Add Hinge Joint on the player with correct settings
        //HingeJoint joint = gameObject.AddComponent<HingeJoint>();
        //joint.axis = Vector3.back; /// (0,0,-1)
        //joint.anchor = Vector3.zero;  // this.transform.position.y*Vector3.up;


        //HingeJoint jointForGrabbable = grabbable.AddComponent<HingeJoint>();
        //jointForGrabbable.axis = Vector3.back;
        //jointForGrabbable.anchor = Vector3.zero; // -Vector3.up*5f;

        //joint.connectedBody = grabbable.GetComponent<Rigidbody>();
        //jointForGrabbable.connectedBody = this.GetComponent<Rigidbody>();
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
        Destroy(grabbable.GetComponent<HingeJoint>());
        Destroy(this.GetComponent<HingeJoint>());
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Grabbable") && Input.GetKey(key) )
        {
            Grab(other.gameObject);
        }
    }
    
}
