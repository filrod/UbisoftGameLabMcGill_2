using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

   private Rigidbody rb;
   private bool isGrabbed; 

   public void Start(){
       rb = GetComponent<Rigidbody>();
   }

   public virtual void Grab(Transform player, Transform defaultTrans){
       obj.transform.position = defaultTrans.position;
       obj.transform.SetParent(player);
       rb.isKinematic = true;
       rb.detectCollisions = false;
       isGrabbed = true;
   }

   public virtual void UnGrab(Transform player, Transform defaultTrans){
        obj.transform.parent = null;
        obj.transform.position  += new Vector3(0.5f, 0.5f, 0.5f);
        rb.isKinematic = false;
        rb.detectCollisions = true;
        isGrabbed = false;
   }

    public void Update(){
       if (CanInteract(key1, player1) && !GetIsGrabbed()){
           Grab(player1, defaultTrans1);
       }
       else if (CanInteract(key2, player2) && !GetIsGrabbed()){
           Grab(player2, defaultTrans2);
       }
       else if (CanInteract(key1, player1) && GetIsGrabbed()){
           UnGrab(player1, defaultTrans1);
       }
       else if (CanInteract(key2, player2) && GetIsGrabbed()){
           UnGrab(player2, defaultTrans2);
       }
   }

   public bool GetIsGrabbed(){
       return isGrabbed;
   }

   public bool CanInteract(KeyCode key, Transform player){
       float distance = Vector3.Distance(player.position, obj.transform.position);
       return Input.GetKeyDown(key) && (distance <= radius);
   }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(obj.transform.position, radius);
    }
}
