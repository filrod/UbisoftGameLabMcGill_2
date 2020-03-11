using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private Scientist scientist;

    public AttackState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
    }

    public override Type TransitionCheck()
    {
        
        //TODO should call some "reset level" script insted of just printing to the console...
        Debug.Log("Player is killed");
        return typeof(WanderState);
        
    }
}
