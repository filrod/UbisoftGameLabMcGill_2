﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomListing : MonoBehaviour
{
    public TextMeshProUGUI _text;

    public RoomInfo RoomInfo
    {
        get;
        private set;
    }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name;
    }

    public void OnClick_Button()
    {
        Debug.Log("Join the room");
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }


}
