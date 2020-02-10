using System;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : BaseState
{

    // TODO
    // Legg til destinasjon
    // Flytt/roter mot destinasjonen
    // Om vi er nærme nok, finn ny destinasjon
    // Gjør noe i triggersetof så dette kan testes



    private Scientist scientist;

    public WanderState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
    }

    public override Type TransitionCheck()
    {

        Vector3? triggerPosition = TriggerSetOf();
        if (triggerPosition != null)
        {
            scientist.SetTargetPosition(triggerPosition);
            return typeof(InvestigateState);
        }


        return null;
    }

    public Vector3? TriggerSetOf()
    {

        return null;
    }
}
