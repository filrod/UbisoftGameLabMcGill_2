using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTether : MonoBehaviour
{
    public Transform newTether;

    public Swing swing;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            swing.pendulum.SwitchTether(newTether.transform.position);
        }
    }

}
