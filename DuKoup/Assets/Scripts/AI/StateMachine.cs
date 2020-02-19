using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{

    // TODO
    // I update så skal man kalle transitioncheck for currstate.
    // Sjekk om state opdateres, i så fall gjør en transition
    // Kanskje legg til en fjerde state (gå til)?

    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private GameObject scientistBody;

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
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        scientistBody.transform.localRotation = Quaternion.Euler(0, 180, 0);
        fieldOfView.Deactivate();

        if (newStateType == typeof(WanderState))
        {
            Debug.Log("Back To Wandering");
            currentState = allStates[newStateType];
        }

        if (newStateType == typeof(AlertState))
        {
            Debug.Log("Alert!");
            currentState = allStates[newStateType];
            ((AlertState)currentState).SetTriggerPositionFromScientist();
        }

        if(newStateType == typeof(InvestigateState))
        {
            Debug.Log("Investigating");
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            //scientistBody.transform.localRotation = Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.identity;
            //gameObject.GetComponent<Scientist>().Trigger(null, null);
            fieldOfView.Activate();
            currentState = allStates[newStateType];
        }
        currentState = allStates[newStateType];
        currentState.TransitionLogic();
    }
}