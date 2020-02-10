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
            // Rotating towards destination
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, rotationSpeed * Time.deltaTime);

            if (transform.rotation.eulerAngles == Vector3.zero)
            {
                return typeof(WanderState); // TODO return swipe state or something. Is in correct position, should look around
            }
        }


        // Checking if we will move out of bounds
        Vector3 nextPos = transform.position - transform.forward * movementSpeed * Time.deltaTime;
        transform.position = nextPos;

        return null;
   
    }

}
