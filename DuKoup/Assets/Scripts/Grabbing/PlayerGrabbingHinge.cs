using UnityEngine;

public class PlayerGrabbingHinge : MonoBehaviour
{
    /// <summary>
    /// The transform of the child component to place grabbed object
    /// </summary>
    [SerializeField][HideInInspector]
    private Transform defaultTrans;

    private GameObject grabPositionObj;
    private PlayerManager playerManager;
    private KeyCode key;
    private bool isGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {

        playerManager = GetComponent<PlayerManager>();

        // Get key code
        if (playerManager.playerId == 1)
            key = KeyCode.E; 
        else
            key = KeyCode.RightShift; 

        foreach (var child in this.GetComponentsInChildren<GameObject>())
        {
            if (child.name == "Grabbed position")
            {
                defaultTrans = child.GetComponent<Transform>();
                grabPositionObj = child;
            }
        }
        if (defaultTrans == null)
        {
            Debug.LogWarning("Grabbed Position not found! Transform of player with offset will be used.");
            defaultTrans = this.transform;
            defaultTrans.position = new Vector3(0f, -0.1f, 2f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (grabPositionObj == null)
        {
            defaultTrans.position = new Vector3(0f, -0.1f, 2f);
        }
        else
        {
            defaultTrans = grabPositionObj.transform;
        }
    }

    private void Grab(Transform transform)
    {
        isGrabbing = true;
        transform.position = defaultTrans.position;

        // Add Hinge Joint on the player with correct settings
        HingeJoint joint = gameObject.AddComponent<HingeJoint>();
        joint.axis = Vector3.back; /// (0,0,-1)
        joint.anchor = Vector3.zero;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Grabbable") && Input.GetKey(key) )
        {
            Grab(other.transform);
        }
    }
}
