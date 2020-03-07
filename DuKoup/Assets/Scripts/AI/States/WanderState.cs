﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : BaseState
{

    private float distAtDestination = 1f;
    private float wanderTimeOut = 4f;
    private float randomTargetRadius = 30;

    private float timer;
    private Vector3 destination;

    private Scientist scientist;
    private NavMeshAgent agent;

    public WanderState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
        agent = scientist.GetComponent<NavMeshAgent>();
        //destination = generateRandomPointWithinBounds(scientist.transform.position);
        //destination = RandomNavSphere(scientist.transform.position, randomTargetRadius, -1);
        timer = wanderTimeOut;  
    }


    public Vector3 generateRandomPointWithinBounds(Vector3 origin)
    {
        Vector2 xBounds = scientist.GetXMovementBounds();
        Vector2 zBounds = scientist.GetZMovementBounds();

        float randX = UnityEngine.Random.Range(xBounds.x, xBounds.y);
        float randZ = UnityEngine.Random.Range(zBounds.x, zBounds.y);

        return new Vector3(randX, origin.y, randZ);
        //NavMeshHit navHit;

        //if (NavMesh.SamplePosition(new Vector3(randX, origin.y, randZ), out navHit, 0, 1 << NavMesh.GetAreaFromName("Walkable")))
        //{
        //    return navHit.position;
        //}

        //return origin;
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randomDirection, out navHit, distance, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            return navHit.position;
        }

        return origin;
    }

    public override Type TransitionCheck()
    {
        if (scientist.TargetIsUpdated())
        {
            return typeof(AlertState);
        }

        timer += Time.deltaTime;

        // Checking if we are at destination or timed out
        if (timer >= wanderTimeOut || agent.remainingDistance <= distAtDestination)
        {
            //Debug.Log("At destination, new destination generated");
            destination = generateRandomPointWithinBounds(scientist.transform.position);
            //destination = RandomNavSphere(scientist.transform.position, randomTargetRadius, -1);
            agent.SetDestination(destination);
            timer = 0;
        }

        return null;
    }

}
