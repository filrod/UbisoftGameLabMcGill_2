using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    // TODO
    // Sjekk at spiller fortsatt er i range?
    // Kall en eller annen animasjon
    // Kall en restart metode på spillere ("kill")
    // Flytt tilbake til start av level


    private Scientist scientist;

    public AttackState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
    }

    public override Type TransitionCheck()
    {

        return null;
    }
}
