using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{

    [SerializeField] protected float maxX;
    [SerializeField] protected float minX;
    [SerializeField] protected float maxZ;
    [SerializeField] protected float minZ;

    [SerializeField] private Transform triggerPos;

    public FieldOfView fieldOfView => GetComponent<FieldOfView>();
    public StateMachine stateMachine => GetComponent<StateMachine>();

    private Vector3? targetPosition = null;

    private void Awake()
    {
        stateMachine.AddState(typeof(WanderState), new WanderState(this));
        stateMachine.AddState(typeof(InvestigateState), new InvestigateState(this));
        stateMachine.AddState(typeof(SwipeState), new SwipeState(this));
        stateMachine.AddState(typeof(AttackState), new AttackState(this));
    }



    public void SetTargetPosition(Vector3? targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public Vector3? GetTargetPosition()
    {
        return targetPosition;
    }

    public Vector3? IsTriggered()
    {
        if (Random.Range(0f, 100f) < 0.5f)
        {
            //return new Vector3(0, transform.localPosition.y, 5);
            return triggerPos.position;
        }

        return null;

    }


    public float GetMinX() { return minX; }
    public float GetMaxX() { return maxX; }
    public float GetMinZ() { return minZ; }
    public float GetMaxZ() { return maxZ; }


}
