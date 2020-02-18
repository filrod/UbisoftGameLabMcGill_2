using System;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : BaseState
{

    private float distAtDestination = 1f;
    private float wanderTimeOut = 3f;

    private float timer;
    private Vector3 destination;

    private Scientist scientist;
    private NavMeshAgent agent;

    public WanderState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        agent = scientist.GetComponent<NavMeshAgent>();
        destination = RandomNavSphere(scientist.transform.position, 10, -1);
        timer = wanderTimeOut;  
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask))
        {
            return navHit.position;
        }

        return origin;
    }

    public override Type TransitionCheck()
    {
        Vector3? triggerPosition = scientist.IsTriggered();
        if (triggerPosition != null)
        {
            scientist.SetTargetPosition(triggerPosition);
            return typeof(InvestigateState);
        }

        timer += Time.deltaTime;

        // Checking if we are at destination or timed out
        if (timer >= wanderTimeOut || agent.remainingDistance <= distAtDestination)
        {
            //Debug.Log("At destination, new destination generated");
            destination = RandomNavSphere(scientist.transform.position, 10, -1);
            agent.SetDestination(destination);
            timer = 0;
        }

        return null;
    }


    public Vector3? TriggerSetOf()
    {

        return null;
    }


}
