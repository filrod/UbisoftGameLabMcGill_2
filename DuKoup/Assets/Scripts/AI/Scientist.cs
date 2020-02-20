using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    [SerializeField]
    [Tooltip ("The z value of where the scientist should stop when he is investigating")] private float scientistZAtTable;
    [SerializeField]
    [Tooltip("The z value of the back of the navmesh")] private float navmeshBackZ;


    [SerializeField]
    [Tooltip ("Script attached to the field of view game object")] private FieldOfView fieldOfView;

    public StateMachine stateMachine => GetComponent<StateMachine>();
    private Vector3? targetPosition = null; // ? indicates a nullable type. If it has a value of null that means the ai has no target and should be wandering
    private Vector3? nextTargetPosition;

    private void Awake()
    {
        stateMachine.AddState(typeof(WanderState), new WanderState(this));
        stateMachine.AddState(typeof(AlertState), new AlertState(this));
        stateMachine.AddState(typeof(InvestigateState), new InvestigateState(this));
        stateMachine.AddState(typeof(AttackState), new AttackState(this));
    }

    // All of this is just to visualize when the scientist is triggered (red face). 
    private void Update()
    {
        if (IsTriggered().HasValue)
        {
            transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            transform.GetChild(0).GetChild(2).GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    // Returns null if the scientist is not triggered
    public Vector3? IsTriggered()
    {
        return targetPosition;
    }

    /// <summary>
    /// Method to check if the scientist has been triggered in a different location. Will also update the target position.
    /// </summary>
    /// <returns> True if the target has been update, false if not</returns>
    public bool TargetIsUpdated()
    {
        if (nextTargetPosition.HasValue)
        {
            if (targetPosition.HasValue && targetPosition.Value.Equals(nextTargetPosition.Value)) // Next target is the same as current target
            {
                nextTargetPosition = null;
                return false;
            }

            targetPosition = nextTargetPosition.Value;
            nextTargetPosition = null;
            return true;
        }
        return false; // We don't have a next target
    }

    /// <summary>
    /// Method to trigger the scientist. What that means is that the "nextTargetPosition" is updated (we dont directly update the target position.
    /// This can happen multiple times before the target position is updated and then (obv) the last Trigger-call is the one taken into account
    /// </summary>
    /// <param name="triggerPosition"></param>
    /// <param name="player"></param>
    public void Trigger(Vector3 triggerPosition, GameObject player)
    {
        nextTargetPosition = new Vector3(triggerPosition.x, transform.position.y, scientistZAtTable);
    }

    public void ResetTargets()
    {
        targetPosition = null;
        //nextTargetPosition = null;
    }

    // Returns true if the field of view is currently hitting player
    public bool FieldOfViewHittingPlayer()
    {
        return fieldOfView.HittingPlayer();
    }

}
