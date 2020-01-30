using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankingBehaviour : MonoBehaviour
{

    private bool inPlankinggSpace = false;
    private GameObject plankingSpace;


    void FixedUpdate()
    {
        if (inPlankinggSpace && Input.GetKeyDown(KeyCode.P))
        {
            Plank();
        }
    }

    private void Plank()
    {
        transform.position = plankingSpace.transform.position;
        transform.eulerAngles = new Vector3(90, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (other.CompareTag("PlankingSpace"))
        {
            Debug.Log("Entered planking space");
            inPlankinggSpace = true;
            plankingSpace = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlankingSpace"))
        {
            inPlankinggSpace = false;
            plankingSpace = null;
        }
    }
    
}
