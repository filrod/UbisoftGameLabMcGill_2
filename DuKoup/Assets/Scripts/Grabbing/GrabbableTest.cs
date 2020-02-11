using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableTest : Grabbable
{
    public override void Grab(Transform player, Transform defaultTrans){
        Debug.Log("grabbed");
        base.Grab(player, defaultTrans);
    }

    public override void UnGrab(Transform player, Transform defaultTrans){
        Debug.Log("ungrabbed");
        base.UnGrab(player, defaultTrans);
    }
   
}
