using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                //Debug.Log("_listings" + _listings);
                //foreach(RoomListing r in _listings)
                //{
                //    if (r.RoomInfo.Name ==info.Name)
                //    {
                //        Destroy(r.gameObject);
                //        _listings.Remove(r);
                //        break;
                //    }
                //}

                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                Debug.Log("index = " + index);
                if (index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            else
            {
                RoomListing listing = Instantiate(_roomListing, _content);
                if (listing != null)
                {
                    listing.SetRoomInfo(info);
                    _listings.Add(listing);
                }
            }
            
        }
    }
}
