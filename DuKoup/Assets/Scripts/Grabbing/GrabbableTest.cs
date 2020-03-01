using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

public class GrabbableTest : Grabbable
{
    private const byte GRAB_EVENT = 0;


    public override void Grab(Transform player, Transform defaultTrans){
        Debug.Log("grabbed");

        base.Grab(player, defaultTrans);
    }

    public override void UnGrab(Transform player, Transform defaultTrans){
        Debug.Log("ungrabbed");
        base.UnGrab(player, defaultTrans);
    }

    public void Update()
    {
        // if (photonView.IsMine && Input.GetKeyDown(KeyCode.Z))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                move();
                // photonView.RPC("RPC_move", RpcTarget.All);
            }
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    public void Awake()
    {
        PhotonView photonView = gameObject.AddComponent<PhotonView>();
        //PhotonTransformView photonTransformationView = gameObject.AddComponent<PhotonTransformView>();
        //photonView.ObservedComponents = new List<Component>();
        //photonView.ObservedComponents.Add(photonTransformationView);

        //foreach (PhotonView v in PhotonNetwork.PhotonViews)
        //{
        //    Debug.Log(gameObject + ": " + "v.owner.actornumber " + v.Owner.ActorNumber + " v.viewID: " + v.ViewID + "localplayer actornumber" + PhotonNetwork.LocalPlayer.ActorNumber);
        //    if (v.Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        //    {
        //        Debug.Log("Allocate " + v.ViewID);
        //        PhotonNetwork.AllocateViewID(v.ViewID);
        //        // photonView.ViewID = v.ViewID;
        //    }
        //}

    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public void move()
    {
        Debug.Log("move1");
        PhotonNetwork.RaiseEvent(GRAB_EVENT, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
    }

    [PunRPC]
    public void RPC_move()
    {
        Debug.Log("move");
        transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {

        
        if (obj.Code == GRAB_EVENT)
        {
            Debug.Log(gameObject + "Event Receive");
            transform.position = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
        }
    }


}
