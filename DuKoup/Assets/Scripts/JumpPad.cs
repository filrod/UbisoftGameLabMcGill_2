using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private float playerJumpForce;
    private Animator animator => GetComponent<Animator>();
    private JumpPadActivationController activationController => GetComponent<JumpPadActivationController>();

 //   private void OnTriggerEnter(Collider other)
	//{
 //       if (other.gameObject.CompareTag("Player"))
 //       {
 //           animator.SetBool("PlayerOnVortex", true);
 //           //other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
 //           playerJumpForce = other.gameObject.GetComponent<PlayerMovement>().JumpForce;
 //           other.gameObject.GetComponent<PlayerMovement>().JumpForce = playerJumpForce * 3;
 //       }
	//}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && activationController.IsActivated())
        {
            animator.SetBool("PlayerOnVortex", true);
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3, 0), ForceMode.Impulse);
        }
    }

        private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("PlayerOnVortex", false);
            //other.gameObject.GetComponent<PlayerMovement>().JumpForce = playerJumpForce;
        }
    }

}
