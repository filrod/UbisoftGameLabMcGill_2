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
    [SerializeField] private float rotationSpeed = 15;

    private float rotationHalfTimeOut = 1.5f;
    private float rotationTimeOut;
    private float rotationTimer = 0;

    private float timeOut = 10;
    private float timer = 0;

    public InvestigateState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        this.scientistEye = scientist.transform.GetChild(1).gameObject;
        currentRotation = leftRotation;
        rotationTimeOut = rotationHalfTimeOut;
    }

    public override Type TransitionCheck()
    {
        if (scientist.TargetIsUpdated()) // We have a new trigger position
        {
            ResetEye();
            return typeof(AlertState);
        }

        if (scientist.FieldOfViewHittingPlayer()) // We have detected a player in the field of view
        {
            ResetEye();
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

    private void ResetEye()
    {
        scientistEye.transform.rotation = Quaternion.Euler(0, 0, -50);
        rotationTimeOut = rotationHalfTimeOut;
        currentRotation = UnityEngine.Random.Range(0f, 1f) < 0.5f ? rightRotation : leftRotation;
    }

    private void ChangeRotation()
    {
        rotationTimeOut = rotationTimeOut == rotationHalfTimeOut ? rotationTimeOut * 2 : rotationTimeOut;
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
