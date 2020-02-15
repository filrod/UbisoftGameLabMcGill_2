using System;
using UnityEngine;

public class WanderState : BaseState
{

    // TODO
    // Legg til destinasjon
    // Flytt/roter mot destinasjonen
    // Om vi er nærme nok, finn ny destinasjon
    // Gjør noe i triggersetof så dette kan testes


    private float movementRange = 10f;
    private float wanderSpeed = 2f;
    private float rotationSpeed = 1f;
    private float distAtDestination = 1f;

    private Vector3 destination;
    private Quaternion desiredRotation;

    private Scientist scientist;

    public WanderState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        destination = GetNewRandMovement();
        
    }

    private Vector3 GetNewRandMovement()
    {
        Vector3 newDirection = new Vector3(UnityEngine.Random.Range(-transform.right.x, transform.right.x), 0, UnityEngine.Random.Range(0, transform.forward.z));

        desiredRotation = Quaternion.LookRotation(newDirection);
        return transform.position + newDirection * 5;
        //Vector3 newDirection = transform.forward;
        ////new Vector3(UnityEngine.Random.Range(0, movementRange), 0, UnityEngine.Random.Range(0, movementRange));
        //float maxNoise = Math.Max(newDirection.x, newDirection.z);
        //newDirection.x += UnityEngine.Random.Range(-maxNoise, maxNoise);
        //newDirection.z += UnityEngine.Random.Range(-maxNoise, maxNoise);
        
        ////newMove = transform.position + newMove.normalized;
        //desiredRotation = Quaternion.LookRotation(newDirection);
        //return transform.position + newDirection * 3;
    }

    public override Type TransitionCheck()
    {
        Debug.DrawLine(transform.position, destination, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.forward * 4, Color.blue);
        // Checking if the scientist is triggered by something
        Vector3? triggerPosition = scientist.IsTriggered();
        if (triggerPosition != null)
        {
            scientist.SetTargetPosition(triggerPosition);
            return typeof(InvestigateState);
        }

        // Checking if we are at destination
        if (Vector3.Distance(transform.position, destination) <= distAtDestination)
        {
            Debug.Log("At destination, new destination generated");
            destination = GetNewRandMovement();
            
        }

        // Checking if we will move out of bounds
        Vector3 nextPos = transform.position + transform.forward * wanderSpeed * Time.deltaTime;
        Vector3 futurePos = nextPos + (transform.forward * wanderSpeed * Time.deltaTime) * 2; // Checking 5 steps ahead 
        if (!IsOutOfBounds(futurePos))
        {
            transform.position = nextPos;
            // Rotating towards destination
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
        else // We should not move out of bounds, maybe just turn around 45 degrees?
        {
            Quaternion rotateIntoBounds = Quaternion.LookRotation(transform.right);
            transform.Rotate(new Vector3(0, 90, 0));//Quaternion.Slerp(transform.rotation, rotateIntoBounds, rotationSpeed * Time.deltaTime);
            destination = GetNewRandMovement();
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
