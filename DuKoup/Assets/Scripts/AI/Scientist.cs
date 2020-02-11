using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{

    [SerializeField] protected float maxX;
    [SerializeField] protected float minX;
    [SerializeField] protected float maxZ;
    [SerializeField] protected float minZ;

    public StateMachine stateMachine => GetComponent<StateMachine>();

    private Vector3? targetPosition = null;

    private void Awake()
    {
        //stateMachine = GetComponent<StateMachine>();
        stateMachine.AddState(typeof(WanderState), new WanderState(this));
        stateMachine.AddState(typeof(InvestigateState), new InvestigateState(this));
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
        if (Random.Range(0f, 100f) < 0)
        {
            return new Vector3(0, transform.position.y, 10);
        }

        return null;

    }


    public float GetMinX() { return minX; }
    public float GetMaxX() { return maxX; }
    public float GetMinZ() { return minZ; }
    public float GetMaxZ() { return maxZ; }


}
