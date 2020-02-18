using System;
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

        Type newStateType = currentState.TransitionCheck();

        if (newStateType != null) // We are changing state
        {
            DoTransition(newStateType);
        }

    }


    private void DoTransition(Type newStateType)
    {
        if (newStateType == typeof(WanderState))
        {
            Debug.Log("Back To Wandering");
            
            currentState = allStates[newStateType];
        }

        if (newStateType == typeof(InvestigateState))
        {
            Debug.Log("Investigating!");
            currentState = allStates[newStateType];
            ((InvestigateState)currentState).SetTriggerPositionFromScientist();
        }

        if(newStateType == typeof(SwipeState))
        {
            Debug.Log("Swiping");
            //transform.GetChild(0).gameObject.GetComponent<FieldOfView>().enabled = true;
            currentState = allStates[newStateType];
        }
        currentState = allStates[newStateType];
        currentState.TransitionLogic();
    }
}