using System;
using System.Collections;
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
    private float rotationSpeed = 1f;
    private float atDestinationDist = 1f;

    private Vector3 desiredRotation;

    private Scientist scientist;

    public InvestigateState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
    }

    public override Type TransitionCheck()
    {
        // Rotating towards destination
        if (Math.Round(transform.forward.x - desiredRotation.x, 1) != 0.0 || Math.Round(transform.forward.z - desiredRotation.z, 1) != 0.0)
        {

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, desiredRotation, rotationSpeed * Time.deltaTime, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
            return null;
        }
        else
        {
            Debug.Log("now rotation is good");
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
            return null;
        }

        // Checking if we are in correct position with correct rotation
        if (Vector3.Distance(transform.position, scientist.GetTargetPosition().Value) < atDestinationDist)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            int x = 0;
            while (x++ < 1000000000) ;

            return typeof(WanderState); // TODO return swipe state or something. Is in correct position, should look around
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
        desiredRotation = (target.Value - transform.position).normalized;
    }

}
