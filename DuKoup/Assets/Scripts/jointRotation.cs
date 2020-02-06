using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jointRotation : MonoBehaviour
{ 
    [SerializeField] private Vector3 pivot = new Vector3();
    [SerializeField] private GameObject rotationPoint;
    [SerializeField] private Vector3 axis = new Vector3();
    [SerializeField] private float angle;
    
    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(rotationPoint.GetComponent<Transform>().position, axis, angle);
    }
}
