using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bob
{
    public Vector3 velocity;
    public float gravity = 20f;
    public Vector3 gravityDirection = new Vector3(0, 1, 0);

    public void ApplyGravity()
    {
        velocity -= gravityDirection * gravity * Time.deltaTime;
    }

}
