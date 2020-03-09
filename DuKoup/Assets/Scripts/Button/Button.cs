using UnityEngine;

public class Button : MonoBehaviour
{

    /// <summary>
    /// The animator attached to the button
    /// </summary>
    private Animator animator => GetComponent<Animator>();
    [SerializeField] private IButtonBehaviour buttonBehavior;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("PushingDown", true);
            buttonBehavior.ButtonDown();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("PushingDown", false);
            buttonBehavior.ButtonUp();
        }
    }
}
