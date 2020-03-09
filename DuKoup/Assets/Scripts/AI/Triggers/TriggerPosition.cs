using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPosition : MonoBehaviour
{

    [SerializeField] private Scientist scientist;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            scientist.Trigger(transform.position, other.gameObject);
        }
    }


}
