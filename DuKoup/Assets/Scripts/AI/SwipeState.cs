using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeState : BaseState
{
    GameObject scientistEye;
 

    [SerializeField] private float maxAngle = 30;
    [SerializeField] private float minAngle = -30;
    [SerializeField] private int nbOfSwipes = 3;
    [SerializeField] private float rotationSpeed = 1;

    private int swipeCount = 0;
    private float goalAngle = 30;

    public SwipeState(Scientist scientist) : base(scientist.gameObject)
    {
        scientistEye = scientist.transform.GetChild(0).gameObject;
    }

    public override Type TransitionCheck()
    {
        scientistEye.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, scientistEye.transform.localRotation.eulerAngles.z));
        if (Math.Round(scientistEye.transform.localRotation.eulerAngles.z - goalAngle, 1) != 0.0)
        {
            
            float currentRotation = gameObject.transform.localRotation.eulerAngles.z;
            //Add current rotation to rotation rate to get new rotation
            Quaternion rotation = Quaternion.Euler(0, 0, currentRotation + rotationSpeed * Time.deltaTime);
            //Move object to new rotation
            scientistEye.transform.localRotation = rotation;

        }

        else
        {
            if (swipeCount == nbOfSwipes)
            {
                swipeCount = 0;
                scientistEye.gameObject.GetComponent<FieldOfView>().enabled = false;
                return typeof(WanderState);
            }

            swipeCount++;
            if (swipeCount == nbOfSwipes)
            {
                goalAngle = 0;
            }
            else
            {
                goalAngle = (goalAngle == maxAngle) ? minAngle : maxAngle;
            }
        }

        return null;

    }

}
