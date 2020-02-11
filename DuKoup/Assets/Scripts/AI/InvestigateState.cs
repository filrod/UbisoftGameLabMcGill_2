using System;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : BaseState
{
    // TODO
    // Gå til target
    // Start blikket på target
    // Sveip høyre og venstre
    // Sjekk for spillere
    // Om man finner spiller, transition til attack
    // Om man har sveipet frem og tilbake et par ganger, transition til wander (slett target i scientist)

    private float movementSpeed = 5f;
    private float rotationSpeed = 3f;
    private float atDestinationDist = 5f;

    private Quaternion desiredRotation;

    private Scientist scientist;

    public InvestigateState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
    }

    public override Type TransitionCheck()
    {
        
        // Checking if we are in correct position with correct rotation
        if (Vector3.Distance(transform.position, scientist.GetTargetPosition().Value) < atDestinationDist)
        {

            if (transform.rotation.eulerAngles == Vector3.zero)
            {
                return typeof(WanderState); // TODO return swipe state or something. Is in correct position, should look around
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1)), rotationSpeed * Time.deltaTime);
            }
        }

        // Rotating towards destination
        if (transform.rotation != desiredRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * (movementSpeed/5) * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
        

        return null;
   
    }


    public override void TransitionLogic()
    {
        SetDesiredRotationFromScientistTarget();
    }


    public void SetDesiredRotationFromScientistTarget()
    {
        Vector3? target = scientist.GetTargetPosition();

        if (target.Value == null) return; // The scientists target position is a nullable vector 3 so we need to check that it has a value
        Debug.Log("Finding rotation");
        desiredRotation = Quaternion.LookRotation(-target.Value);
    }

}
