using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> rope;

    [SerializeField]
    public Vector3 initialForce = new Vector3(0, 0, 0);

    [SerializeField]
    public int ropePieceNum = 5;

    [SerializeField]
    public float ropePieceMass = 0.1f;

    [SerializeField]
    public Vector3 ropePieceSize = new Vector3(1, 1, 1);

    [SerializeField]
    public float colliderRadius = 2f;

    public void RopeSetUp()
    {
        Debug.Log("RopeSetUp");
        Vector3 offset = new Vector3();
        for (int i = 0; i < ropePieceNum; i++)
        {
            GameObject ropePiece = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ropePiece.transform.localScale = ropePieceSize;
            ropePiece.transform.SetParent(transform);
            ropePiece.transform.localPosition = new Vector3();
            ropePiece.transform.localPosition -= offset;
            // ropePiece.AddComponent<CapsuleCollider>();
            Rigidbody rigidBody = ropePiece.AddComponent<Rigidbody>();
            rigidBody.mass = ropePieceMass;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;
            HingeJoint hingeJoint = ropePiece.AddComponent<HingeJoint>();
            hingeJoint.useSpring = true;
            if (i == 0)
            {
                hingeJoint.connectedBody = GetComponent<Rigidbody>();
            }
            else
            {
                hingeJoint.connectedBody = rope[i - 1].GetComponent<Rigidbody>();
            }
            

            rope.Add(ropePiece);
            offset += new Vector3(0, 2f * ropePieceSize.y, 0);
        }

        CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
        collider.center = new Vector3(0, -2f * ropePieceSize.y * ropePieceNum, 0);
        collider.radius = colliderRadius;
        collider.isTrigger = true;

        // Add initial velocity
        rope[ropePieceNum - 1].GetComponent<Rigidbody>().AddForce(initialForce, ForceMode.Impulse);
    }

    private void Awake()
    {
        RopeSetUp();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            if (!playerManager.isGrabRope)
            {
                playerManager.GrabRope(this);
                playerManager.isGrabRope = true;
            }
            
        }
    }
}
