using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class playerInstantiate : MonoBehaviour
{


    private void Awake()
    {
        Vector2 offset = Random.insideUnitCircle * 3f;
        Vector3 position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
        PhotonNetwork.Instantiate("player", position, Quaternion.identity);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
