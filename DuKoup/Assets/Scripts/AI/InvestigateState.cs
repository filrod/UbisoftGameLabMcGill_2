using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    private float atDestinationDist = 0.1f;

    private Vector3 desiredRotation;
    private Vector3 targetPosition;

    private Scientist scientist;
    private NavMeshAgent agent;

    public InvestigateState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        this.agent = scientist.gameObject.GetComponent<NavMeshAgent>();
    }

    public override Type TransitionCheck()
    {
        Debug.Log("In investigation loop");
        Debug.Log(agent.destination);
        if (scientist.transform.position.x - targetPosition.x <= atDestinationDist && scientist.transform.position.z - targetPosition.z <= atDestinationDist)
        {
            Debug.Log("We are at destination");
            scientist.transform.localRotation = Quaternion.Euler(0, 180, 0);

            return typeof(SwipeState);
        }

        if (agent.destination == null)
        {
            SetTriggerPositionFromScientist();
        }

        return null;
    }

    public void SetTriggerPositionFromScientist()
    {
        Vector3? target = scientist.GetTargetPosition();

        if (target.Value == null)
        {
            Debug.Log("Scientist doesn't have trigger target");
            return; // The scientists target position is a nullable vector 3 so we need to check that it has a value
        }
        targetPosition = target.Value;
        agent.SetDestination(targetPosition);
        Debug.Log(agent.destination);
        Debug.Log(targetPosition);
    }

}
