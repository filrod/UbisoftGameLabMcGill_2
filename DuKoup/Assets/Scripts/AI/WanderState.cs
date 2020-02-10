using System;
using UnityEngine;

public class WanderState : BaseState
{

    // TODO
    // Legg til destinasjon
    // Flytt/roter mot destinasjonen
    // Om vi er nærme nok, finn ny destinasjon
    // Gjør noe i triggersetof så dette kan testes


    private float movementRange = 4.5f;
    private float wanderSpeed = 3f;
    private float rotationSpeed = 3f;
    private float distAtDestination = 1f;

    private Vector3 randMovement;
    private Quaternion desiredRotation;

    private Scientist scientist;
    private Vector3? triggerDestination = null;

    public WanderState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        randMovement = GetNewRandMovement();
        
    }

    private Vector3 GetNewRandMovement()
    {
        Vector3 newMove = -transform.forward + new Vector3(UnityEngine.Random.Range(-movementRange, movementRange), 0, UnityEngine.Random.Range(-movementRange, movementRange));
        desiredRotation = Quaternion.LookRotation(randMovement);
        return newMove.normalized;
    }

    public override Type TransitionCheck()
    {

        Vector3? triggerPosition = TriggerSetOf();
        if (triggerPosition != null)
        {
            scientist.SetTargetPosition(triggerPosition);
            return typeof(InvestigateState);
        }

        // Checking if we are at destination
        if (Vector3.Distance(transform.position, transform.position + randMovement) <= distAtDestination)
        {
            randMovement = GetNewRandMovement();
            
        }

        // Rotating towards destination
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);

        // Checking if we will move out of bounds
        Vector3 nextPos = transform.position + transform.forward * wanderSpeed * Time.deltaTime;
        if (!IsOutOfBounds(nextPos))
        {
            transform.position = nextPos;
        }
        else // We should not move out of bounds, maybe just turn around 45 degrees?
        {
            Quaternion rotateIntoBounds = Quaternion.LookRotation(transform.right);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotateIntoBounds, rotationSpeed);
            randMovement = GetNewRandMovement();
        }



        return null;
    }

    private bool IsOutOfBounds(Vector3 position)
    {
        return position.x < scientist.GetMinX() || position.x > scientist.GetMaxX() || position.z < scientist.GetMinZ() || position.z > scientist.GetMaxZ();
    }

    public Vector3? TriggerSetOf()
    {

        return null;
    }
}
