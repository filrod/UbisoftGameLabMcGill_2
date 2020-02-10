using System;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : BaseState
{
    // TODO
    // Gå til target
    // Start blikket på target
    // Sveip høyre og venstre
    // Sjekk for spillere
    // Om man finner spiller, transition til attack
    // Om man har sveipet frem og tilbake et par ganger, transition til wander (slett target i scientist)


    private Scientist scientist;

    public InvestigateState(Scientist scientist) : base(scientist.gameObject)
    {
        this.scientist = scientist;
    }

    public override Type TransitionCheck()
    {

        return null;
    }
}
