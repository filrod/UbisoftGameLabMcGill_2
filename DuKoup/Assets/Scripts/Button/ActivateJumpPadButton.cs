using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateJumpPadButton : IButtonBehaviour
{
    [SerializeField]
    [Tooltip("The jump pad to activate")] private GameObject jumpPad;
    private JumpPadActivationController activationController;

    private void Start()
    {
        activationController = jumpPad.GetComponent<JumpPadActivationController>();
    }

    public override void ButtonDown()
    {
        activationController.SetJumpPadActive(true);
    }

    public override void ButtonUp()
    {
        activationController.SetJumpPadActive(false);
    }
}
