using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadActivationController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("True if the jumppad should start activated, false otherwise")] private bool activated = false;

    private Animator animator => GetComponent<Animator>();

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("Activated", activated);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Activated", activated);
    }

    public void SetJumpPadActive(bool activation)
    {
        activated = activation;
    }

    public bool IsActivated()
    {
        return activated;
    }
}
