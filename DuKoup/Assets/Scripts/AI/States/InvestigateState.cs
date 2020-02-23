using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : BaseState
{

    Scientist scientist;

    private float timeOut = 4;
    private float timer = 0;

    public InvestigateState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
    }

    public override Type TransitionCheck()
    {
        if (scientist.TargetIsUpdated()) // We have a new trigger position
        {
            return typeof(AlertState);
        }

        if (scientist.FieldOfViewHittingPlayer()) // We have detected a player in the field of view
        {
            scientist.ResetTargets(); // Resetting the scientist's target
            return typeof(AttackState);
        }

        // Correcting for weird navmesh behaviour
        Vector3? correctScientistPos = scientist.IsTriggered();
        if (correctScientistPos.HasValue) scientist.transform.position = new Vector3(scientist.transform.position.x, scientist.transform.position.y, correctScientistPos.Value.z);

        // Scientist should time out of the investigation if it does not detect any players for a certain amount of time
        timer += Time.deltaTime;
        if (timer >= timeOut)
        {
            timer = 0;
            scientist.ResetTargets(); // Resetting the scientist's target
            return typeof(WanderState);
        }

        return null;
    }

}
