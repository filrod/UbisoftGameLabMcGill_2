using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    private string _roomName;

    private RoomsCanvases _roomsCanvases;

    string RandomString (){
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string returnString = "";

        for (int i = 0; i<5; i++) {

            returnString += chars[Random.Range(0, chars.Length)];
        }
        return returnString;
    }  

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        // CreateRoom
        // JoinOrCreateRoom
        // If the room exist, then join the room instead of creating
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        // two players
        options.MaxPlayers = 3;
        _roomName = RandomString();
        PhotonNetwork.JoinOrCreateRoom(_roomName, options, TypedLobby.Default);

    }

    public override void OnCreatedRoom()
    {
        base.OnConnected();
        Debug.Log("Created Room Successfully");
        _roomsCanvases.CurrentRoomCanvas.Show();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Created Room failed");
    }

    public void Update()
    {
        if (Input.GetButtonDown("CreateRoom"))
        {
            CreateRoom();
        }
    }
}
