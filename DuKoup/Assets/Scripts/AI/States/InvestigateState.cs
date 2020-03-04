using System;
using System.Collections;
using UnityEngine;

public class InvestigateState : BaseState
{

    Scientist scientist;
    GameObject scientistEye;

    private Vector3 leftRotation = new Vector3(0, 0, 1);
    private Vector3 rightRotation = new Vector3(0, 0, -1);
    private Vector3 currentRotation;
    [SerializeField] private float rotationSpeed = 10;

    private float rotationTimeOut = 3f;
    private float rotationTimer = 0;

    private float timeOut = 10;
    private float timer = 0;

    public InvestigateState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        this.scientistEye = scientist.transform.GetChild(1).gameObject;
    }

    public override Type TransitionCheck()
    {
        if (scientist.TargetIsUpdated()) // We have a new trigger position
        {
            return typeof(AlertState);
        }

        if (scientist.FieldOfViewHittingPlayer()) // We have detected a player in the field of view
        {
            scientist.ResetTargets(); // Resetting the scientist's target
            return typeof(AttackState);
        }

        // Correcting for weird navmesh behaviour
        Vector3? correctScientistPos = scientist.IsTriggered();
        if (correctScientistPos.HasValue) scientist.transform.position = new Vector3(scientist.transform.position.x, scientist.transform.position.y, correctScientistPos.Value.z);

        // Scientist should time out of the investigation if it does not detect any players for a certain amount of time
        timer += Time.deltaTime;
        if (timer >= timeOut)
        {
            timer = 0;
            scientist.ResetTargets(); // Resetting the scientist's target
            return typeof(WanderState);
        }

        rotationTimer += Time.deltaTime;
        if (rotationTimer >= rotationTimeOut)
        {
            rotationTimer = 0;
            ChangeRotation();
        }

        scientistEye.transform.Rotate(currentRotation * Time.deltaTime * rotationSpeed);

        return null;
    }



    private void ChangeRotation()
    {
        switch (currentRotation.Equals(leftRotation))
        {
            case true:
                currentRotation = rightRotation;
                break;
            case false:
                currentRotation = leftRotation;
                break;
        }
    }

}
