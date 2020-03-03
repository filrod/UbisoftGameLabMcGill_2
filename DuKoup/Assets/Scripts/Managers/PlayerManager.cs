using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Cinemachine;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPun
{

    /// <summary> Player identification for distiction between player 1 and 2 (serialized) </summary>
    // [SerializeField] private int playerId;
    // Used PhotonNetwork.LocalPlayer.ActorNumber instead
    [SerializeField]
    [Tooltip("A number, either 1 or 2, to say which player this is. This is used for player input managment")]
    public int playerId;

    public static GameObject LocalPlayerInstance;

    /// <summary>
    /// A player holds a reference of another player
    /// </summary>
    private GameObject otherPlayer = null;

    public GameObject OtherPlayer
    {
        get
        {
            if (otherPlayer == null)
            {
                foreach (PlayerManager manager in FindObjectsOfType<PlayerManager>())
                {
                    if (manager == this)
                    {
                        continue;
                    }
                    otherPlayer = manager.gameObject;
                    return otherPlayer;
                }
            }
            return otherPlayer;
        }

        set
        {
            otherPlayer = value;
        }
    }

    [SerializeField]
    public PlayerMovement playerMovement;
    [SerializeField]
    public PlankingSupport plankingSupport;
    [SerializeField]
    public Pushing pushing;
    [SerializeField]
    public PlayersCollision playerCollision;
    [SerializeField]
    public PlankingBehaviour plankingBahaviour;

    public void Awake()
    {
        // TODO: remove dummy
        plankingSupport = GetComponentInChildren<PlankingSupport>();
        pushing = GetComponentInChildren<Pushing>();
        playerMovement = GetComponentInChildren<PlayerMovement>();
        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }
        plankingBahaviour = GetComponentInChildren<PlankingBehaviour>();
        if (plankingBahaviour == null)
        {
            plankingBahaviour = GetComponent<PlankingBehaviour>();
        }
        playerCollision = GetComponentInChildren<PlayersCollision>();
        if (!playerCollision)
        {
            playerCollision = GetComponent<PlayersCollision>();
        }

        // Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        if (PhotonNetwork.IsConnected)
        {
            playerId = GetComponent<PhotonView>().Owner.ActorNumber;
        }
    }

    public void Start()
    {
        CinemachineTargetGroup cinemachineTargetGroup = FindObjectOfType<CinemachineTargetGroup>();
        if (cinemachineTargetGroup == null)
        {
            Debug.Log("Missing Camera Prefab");
        }
        else
        {
            if (playerId == 1)
            {
                Debug.Log("player 1 setting");
                cinemachineTargetGroup.m_Targets[0].target = GetComponentInChildren<PlayerMovement>().gameObject.transform;
            }
            else
            {
                Debug.Log("player 2 setting");
                cinemachineTargetGroup.m_Targets[1].target = GetComponentInChildren<PlayerMovement>().gameObject.transform;
            }
        }
        
        
    }
}
