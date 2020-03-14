using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomMenu : MonoBehaviour
{
    private RoomsCanvases _roomCanvas;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvas = canvases;
    }


    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        _roomCanvas.CurrentRoomCanvas.Hide();
        _roomCanvas.CreateOrJoinRoomCanvas.gameObject.SetActive(true);
    }


}
