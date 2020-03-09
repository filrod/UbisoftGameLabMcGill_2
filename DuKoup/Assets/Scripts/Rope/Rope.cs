using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    public List<HingeJoint> joints;

    CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
        HingeJoint[] hingeJoint = GetComponentsInChildren<HingeJoint>();
        joints.AddRange(hingeJoint);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.GrabRope(this);
        }
    }
}
