using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    /// <summary> Player identification for distiction between player 1 and 2 (serialized) </summary>
    // [SerializeField] private int playerId;
    // Used PhotonNetwork.LocalPlayer.ActorNumber instead
    [SerializeField]
    [Tooltip("A number, either 1 or 2, to say which player this is. This is used for player input managment")]
    public int playerId;

    /// <summary>
    /// A player holds a reference of another player
    /// </summary>
    [SerializeField]
    private GameObject otherPlayer;

    public GameObject OtherPlayer
    {
        get
        {
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

    private void Awake()
    {
        foreach (PlayerManager manager in FindObjectsOfType<PlayerManager>())
        {
            if (manager == this)
            {
                continue;
            }
            OtherPlayer = manager.gameObject;
        }

        plankingSupport = GetComponentInChildren<PlankingSupport>();
        pushing = GetComponentInChildren<Pushing>();
        playerMovement = GetComponentInChildren<PlayerMovement>();
        plankingBahaviour = GetComponentInChildren<PlankingBehaviour>();
        playerCollision = GetComponentInChildren<PlayersCollision>();
        
    }
}
