using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTriggeringAI : MonoBehaviour
{
    [SerializeField] private float threasholdValue;
    [SerializeField] private Scientist ai;

    private Rigidbody rb => GetComponent<Rigidbody>();


    // Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(rb.velocity) >= threasholdValue)
        {
            Debug.Log(transform.position);
            ai.Trigger(transform.position, null);
        }
    }
}
