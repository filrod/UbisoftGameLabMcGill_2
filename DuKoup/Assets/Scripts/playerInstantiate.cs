using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class playerInstantiate : MonoBehaviourPun
{
    PlayerManager[] playerManagers;
    GameObject player1;
    GameObject player2;

    private void Awake()
    {
        Vector2 offset = Random.insideUnitCircle * 5f;
        Vector3 position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z);
        
        PhotonNetwork.Instantiate("player", position, Quaternion.identity);

    }
}
