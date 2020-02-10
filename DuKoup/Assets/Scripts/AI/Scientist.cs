using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
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

}
