using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : BaseState
{
    GameObject scientistEye;
    Scientist scientist;

    //[SerializeField] private float maxAngle = 30;
    //[SerializeField] private float minAngle = -30;
    //[SerializeField] private int nbOfSwipes = 3;
    //[SerializeField] private float rotationSpeed = 3;

    //private int swipeCount = 0;
    //private float goalAngle = 30;
    private float timeOut = 4;
    private float timer = 0;

    public InvestigateState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        scientistEye = scientist.transform.GetChild(0).gameObject;
    }

    public override Type TransitionCheck()
    {
        if (scientist.TargetIsUpdated()) // We have a new trigger position
        {
            //scientist.ResetTargets();
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

        return null;
        //scientistEye.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, scientistEye.transform.localRotation.eulerAngles.z));
        //if (Math.Round(scientistEye.transform.localRotation.eulerAngles.z - goalAngle, 1) != 0.0)
        //{
            
        //    float currentRotation = gameObject.transform.localRotation.eulerAngles.z;
        //    //Add current rotation to rotation rate to get new rotation
        //    Quaternion rotation = Quaternion.Euler(0, 0, currentRotation + rotationSpeed * Time.deltaTime);
        //    //Move object to new rotation
        //    scientistEye.transform.localRotation = Quaternion.Euler(Vector3.RotateTowards(gameObject.transform.localRotation.eulerAngles, new Vector3(0, 0, goalAngle), 1, 1));

        //}

        //else
        //{
        //    if (swipeCount == nbOfSwipes)
        //    {
        //        swipeCount = 0;
        //        scientistEye.gameObject.GetComponent<FieldOfView>().enabled = false;
        //        return typeof(WanderState);
        //    }

        //    swipeCount++;
        //    if (swipeCount == nbOfSwipes)
        //    {
        //        goalAngle = 0;
        //    }
        //    else
        //    {
        //        goalAngle = (goalAngle == maxAngle) ? minAngle : maxAngle;
        //    }
        //}

        //return null;

    }

}
