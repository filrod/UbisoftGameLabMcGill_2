using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (base.photonView.IsMine)
        {
            float x = Input.GetAxisRaw("Horizontal1");
            float y = Input.GetAxisRaw("Vertical1");
            transform.position += new Vector3(x, y, 0f) * 0.2f;
        }
    }
}
