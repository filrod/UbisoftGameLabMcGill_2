using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pendulum
{
    public Transform bob_tr;
    public Tether tether;
    public Arm arm;
    public Bob bob;

    public void Initialize()
    {
        bob_tr.transform.parent = tether.tether_tr;
        arm.length = Vector3.Distance(bob_tr.transform.localPosition, tether.position);
    }

    public Vector3 MoveBob(Vector3 pos, float deltaTime)
    {
        bob.ApplyGravity();

        pos += bob.velocity * deltaTime;

        return pos;
    }
}
