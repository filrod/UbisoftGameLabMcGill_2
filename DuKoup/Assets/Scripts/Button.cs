using UnityEngine;

public class Button : MonoBehaviour
{

    Animator animator => GetComponent<Animator>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("PushingDown", true);
            ButtonAction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("PushingDown", false);
            ButtonUnAction();
        }
    }

    private void ButtonAction()
    {
        Debug.Log("Pressed button");
    }

    private void ButtonUnAction()
    {
        Debug.Log("Unpressed button");
    }
}
