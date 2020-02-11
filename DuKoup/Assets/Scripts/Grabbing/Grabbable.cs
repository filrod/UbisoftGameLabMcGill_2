using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
///  Abstract class that every objects that you can grab inherits
///   To change the Grabbing and Ungrabbing mechanics,  override Grab() and UnGrab()
///
///   Change key1 to change the grabbing control of player1, key2 for player2
///
///   Note: We should change the UnGrab() transform position vector
///   To add: Controling the object with mouse when it is grabbed
/// </summary> 

public abstract class Grabbable : MonoBehaviour
{
  
   [SerializeField] private GameObject obj;

   [SerializeField] private Transform player1;
   [SerializeField] private Transform defaultTrans1;

   [SerializeField] private Transform player2;
   [SerializeField] private Transform defaultTrans2;

   [SerializeField] private float radius = 0.1f; 

   [SerializeField] private KeyCode key1 = KeyCode.E;
   [SerializeField] private KeyCode key2 = KeyCode.L;

   
   [SerializeField] private Collider2D area1;
   [SerializeField] private Collider2D area2;

    private Vector3 screenPoint;
	private Vector3 offset;

    private Rigidbody rb;
    private bool isGrabbed; 
    private bool grabbedBy1;
    private bool grabbedBy2;

    // First method called, get rigidbody of the grabbable object
    public void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    // At each image, we want to check if the player wants to grab or ungrab the object.
    public void Update()
    {
        // Grab: for each player that wants and can grab it.
       if (CanInteract(key1, player1) && !isGrabbed){
            Grab(player1, defaultTrans1);
            grabbedBy1 = true;
       }
       else if (CanInteract(key2, player2) && !isGrabbed){
            Grab(player2, defaultTrans2);
            grabbedBy2 = true;
       }

       // Ungrab
       else if (Input.GetKeyDown(key1)&& isGrabbed){
           UnGrab(player1, defaultTrans1);
       }
       else if (Input.GetKeyDown(key2) && isGrabbed){
           UnGrab(player2, defaultTrans2);
       }
   }

   public virtual void Grab(Transform player, Transform defaultTrans)
   {
        // Move object to the leaves
       obj.transform.position = defaultTrans.position;
       obj.transform.SetParent(player);

        // Keep track of the object position to drag it 
       screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
	   offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        // Make it kinematic so that it doesn't mess everything up.
        rb.isKinematic = true;
        rb.detectCollisions = false;
        isGrabbed = true;
   }

   public virtual void UnGrab(Transform player, Transform defaultTrans){

       // We eject it
        obj.transform.parent = null;
        obj.transform.position  += new Vector3(0, 0.7f, 0);

        // It becomes a traditional rigidbody
        rb.isKinematic = false;
        rb.detectCollisions = true;
        isGrabbed = false;
   }

    // If the object is grabbed, we want to drag it.
    public void FixedUpdate(){
        if (isGrabbed){
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		    Vector3 cursorPosition = (Camera.main.ScreenToWorldPoint(cursorPoint) + offset );
		    transform.position = cursorPosition;
        }
    }

    // Method to check if the player has hit the correct input and is at the correct location to grab the object.
   public bool CanInteract(KeyCode key, Transform player)
   {
       float distance = Vector3.Distance(player.position, obj.transform.position);
       return Input.GetKeyDown(key) && (distance <= radius);
   }

    // Restrict grabbed object in area

    void LateUpdate(){
        if (grabbedBy1){
            restrictObject(area1);
        }
        else if (grabbedBy2){
            restrictObject(area2);
        }
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

    // Draw interaction radius in the inspector. For developpers. 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(obj.transform.position, radius);
    }
}
