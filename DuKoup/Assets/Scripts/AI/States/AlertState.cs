using System;
using UnityEngine;
using UnityEngine.AI;

public class AlertState : BaseState
{

    private float atDestinationDist = 0.05f;

    private Vector3 targetPosition;

    private Scientist scientist;
    private NavMeshAgent agent;

    public AlertState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        this.agent = scientist.gameObject.GetComponent<NavMeshAgent>();
    }

    public override Type TransitionCheck()
    {
        if (scientist.TargetIsUpdated()) return typeof(AlertState); // 'Restarting' the alert state with a new position

        if (agent.destination == null)
        {
            SetTriggerPositionFromScientist();
            return null;
        }

        if (scientist.transform.position.x - targetPosition.x <= atDestinationDist && scientist.transform.position.z - targetPosition.z <= atDestinationDist)
        {

            return typeof(InvestigateState);
        }

        return null;
    }

    public void SetTriggerPositionFromScientist()
    {
        Vector3? target = scientist.IsTriggered();

        if (!target.HasValue)
        {
            
            Debug.Log("Scientist doesn't have trigger target");
            return; // The scientists target position is a nullable vector 3 so we need to check that it has a value
        }
        targetPosition = target.Value;
        agent.SetDestination(targetPosition);
    }

}
