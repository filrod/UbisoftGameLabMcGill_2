using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    [SerializeField] private float scientistZAtTable;
    //[SerializeField] private Transform triggerPos;

    [SerializeField] private FieldOfView fieldOfView;
    public StateMachine stateMachine => GetComponent<StateMachine>();

    private Vector3? targetPosition = null;
    private Vector3? nextTargetPosition;

    private void Awake()
    {
        stateMachine.AddState(typeof(WanderState), new WanderState(this));
        stateMachine.AddState(typeof(AlertState), new AlertState(this));
        stateMachine.AddState(typeof(InvestigateState), new InvestigateState(this));
        stateMachine.AddState(typeof(AttackState), new AttackState(this));
    }

    private void Update()
    {
        if (IsTriggered().HasValue)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.black;
        }
    }

    public Vector3? IsTriggered()
    {
        return targetPosition;
    }

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

    public void Trigger(Vector3 triggerPosition, GameObject player)
    {
        nextTargetPosition = new Vector3(triggerPosition.x, transform.position.y, scientistZAtTable);
    }

    public void ResetTargets()
    {
        targetPosition = null;
        //nextTargetPosition = null;
    }

    public bool FieldOfViewHittingPlayer()
    {
        return fieldOfView.HittingPlayer();
    }




}
