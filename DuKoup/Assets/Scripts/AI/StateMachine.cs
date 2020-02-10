﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    // TODO
    // I update så skal man kalle transitioncheck for currstate.
    // Sjekk om state opdateres, i så fall gjør en transition
    // Kanskje legg til en fjerde state (gå til)?


    private Dictionary<Type, BaseState> allStates = new Dictionary<Type, BaseState>();

    public BaseState currentState;
    public event Action<BaseState> OnStateTransition;

    public void AddState(Type type, BaseState state)
    {
        allStates.Add(type, state);
    }

    private void Start()
    {
        if (allStates.ContainsKey(typeof(WanderState)))
        {
            currentState = allStates[typeof(WanderState)];
        }
    }


    private void Update()
    {
        if (currentState == null)
        {
            currentState = allStates[typeof(WanderState)];
        }

    }
}
