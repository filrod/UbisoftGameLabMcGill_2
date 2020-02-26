using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{

    [SerializeField]
    [Tooltip ("Script attached to the FieldOfView game object")] private FieldOfView fieldOfView;
    [SerializeField]
    [Tooltip ("The game object holding the body of the scientist")]private GameObject scientistBody;

    private Dictionary<Type, BaseState> allStates = new Dictionary<Type, BaseState>(); // Holding an instance of each state

    public BaseState currentState; // Reference to the instance of the state we are in

    /// <summary>
    /// Method to add state instances to the dictionary holding all state instances. This is only done by the scientist when it is instantiated
    /// </summary>
    /// <param name="type"></param> 
    /// <param name="state"></param>
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

    /// <summary>
    /// Method to do some transition logic between state changes. Called each time the ai changes it's state.
    /// </summary>
    /// <param name="newStateType"></param>
    private void DoTransition(Type newStateType)
    {
        // These are the general "settings" (only changed for when scientis is investigating).
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        scientistBody.transform.localRotation = Quaternion.Euler(0, 0, 0);
        fieldOfView.Deactivate();

        if (newStateType == typeof(WanderState))
        {
            //Debug.Log("Back To Wandering");
            currentState = allStates[newStateType];
        }

        if (newStateType == typeof(AlertState))
        {
            //Debug.Log("Alert!");
            currentState = allStates[newStateType];
            ((AlertState)currentState).SetTriggerPositionFromScientist();
        }

        if (newStateType == typeof(InvestigateState))
        {
            //Debug.Log("Investigating");
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            transform.rotation = Quaternion.Euler(0, 180, 0); // Rotate to look at level. Kinda janky, just jumps to correct rotation, but the jump normally is not very big
            fieldOfView.Activate();
            currentState = allStates[newStateType];
        }

        if (newStateType == typeof(AttackState))
        {
            currentState = allStates[newStateType];
        }
    }
}