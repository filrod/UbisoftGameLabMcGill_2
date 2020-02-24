using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Robin is the best

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
  
   [SerializeField] [Tooltip("Object you want the player to grab. Drag the object in the hierarchy in this spot.")] private GameObject obj;

   [SerializeField] [Tooltip("Drag dummy1 in this spot")] private Transform player1;
   [SerializeField] [Tooltip("It is an empty Game Object, child of player 1, that defines the position of the object once grabbed by Player1.")] private Transform defaultTrans1;

   [SerializeField] [Tooltip("Drag dummy2 in this spot")] private Transform player2;
   [SerializeField] [Tooltip("It is an empty Game Object, child of player 2, that defines the position of the object once grabbed by Player2.")] private Transform defaultTrans2;

   [SerializeField] [Tooltip("Sphere around the object where the player can interact with the object.")] private float radiusOfInteraction = 2f; 

//    [SerializeField] [Tooltip("Control button for player 1 to grab the object")] private KeyCode grabbingInputPlayer1 = ;
//    [SerializeField] [Tooltip("Control button for player 2 to grab the object")] private KeyCode grabbingInputPlayer2 = KeyCode.E;

   
   //[SerializeField] private Collider2D area1;
   //[SerializeField] private Collider2D area2;


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

    /// <summary>
    /// UBISOFT GAMES LAB - McGill Team #2
    /// -----------------------------------
    /// @author Robin Leman
    /// @Date 2020/02/17
    ///
    /// At each image, we want to check if the player wants to grab or ungrab the object. We then call the method Grab() or UnGrab().
    ///
    /// </summary>
    public void Update()
    {
        // Grab: for each player that wants and can grab it.
       if (CanInteract(player1) && !isGrabbed){
            Grab(player1, defaultTrans1);
            grabbedBy1 = true;
       }
       else if (CanInteract(player2) && !isGrabbed){
            Grab(player2, defaultTrans2);
            grabbedBy2 = true;
       }

       // Ungrab
       else if (Input.GetButtonDown("Grab1")&&  grabbedBy1){
           UnGrab(player1, defaultTrans1);
           grabbedBy1 = false;
       }
       else if (Input.GetButtonDown("Grab2") && grabbedBy2){
           UnGrab(player2, defaultTrans2);
           grabbedBy2 = true;
       }
   }

    /// <summary>
    /// UBISOFT GAMES LAB - McGill Team #2
    /// -----------------------------------
    /// @author Robin Leman
    /// @Date 2020/02/17
    ///
    /// Meant to be overwritten. Set the object as a child of the player, change its position to the default transform position and remove gravity on it.
    ///
    /// </summary>
   public virtual void Grab(Transform player, Transform defaultTrans)
   {
        // Move object to the leaves
       obj.transform.position = defaultTrans.position;
       obj.transform.SetParent(player);

        // Keep track of the object position to drag it 
       //screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
	   //offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        // Make it kinematic so that it doesn't mess everything up.
        rb.isKinematic = true;
        rb.detectCollisions = false;
        isGrabbed = true;
   }

    /// <summary>
    /// UBISOFT GAMES LAB - McGill Team #2
    /// -----------------------------------
    /// @author Robin Leman
    /// @Date 2020/02/17
    ///
    /// Meant to be overwritten. Unchild the object to the player, ejects the object above the player and reset gravity on it.
    ///
    /// </summary>
   public virtual void UnGrab(Transform player, Transform defaultTrans){

       // We eject it
        obj.transform.parent = null;
        obj.transform.position  += new Vector3(0, 0.7f, 0);

        // It becomes a traditional rigidbody
        rb.isKinematic = false;
        rb.detectCollisions = true;
        isGrabbed = false;
   }

    // Method to check if the player has hit the correct input and is at the correct location to grab the object.
   public bool CanInteract(Transform player)
   {
       string key;

        if (player.name == "dummy1"){
           key = "Grab1";
       }
       else{
           key = "Grab2";
       }
       float distance = Vector3.Distance(player.position, obj.transform.position);
       return Input.GetButtonDown(key) && (distance <= radiusOfInteraction) ;
   }

    /////// UNCOMMENT TO HAVE DRAGGING
    /*


     // If the object is grabbed, we want to drag it.
    public void FixedUpdate(){
        if (isGrabbed){
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		    Vector3 cursorPosition = (Camera.main.ScreenToWorldPoint(cursorPoint) + offset );
		    transform.position = cursorPosition;
        }
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



    */

    // Draw interaction  in the inspector. For developpers. 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(obj.transform.position, radiusOfInteraction);
    }
}
