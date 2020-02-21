using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpot : MonoBehaviour
{
    /// <summary>
    /// The player who enter the power up spot
    /// </summary>
    private GameObject enteredPlayer;


    enum POWER_TYPE
    {
        Low_Gravity,
        Fast_Speed,
        Grabable,
        High_Jump,
        Default
    }

    [SerializeField]
    private POWER_TYPE currentPowerType = POWER_TYPE.Default;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(powerUpSpotCollider.isTrigger);
    }

    public void OnCollisionEnter(Collision collision)
    {
        // TODO: refactor the dummy, and unify the tag
        if (collision.gameObject.CompareTag("Player"))
        {
            enteredPlayer = collision.gameObject;

            // player power up
            PowerUp powerUp = enteredPlayer.GetComponent<PowerUp>();

            switch (currentPowerType)
            {
                case POWER_TYPE.Default:
                    powerUp.Reset();
                    break;
                case POWER_TYPE.High_Jump:
                    powerUp.GainHighJump();
                    break;
                case POWER_TYPE.Grabable:
                    powerUp.GainGrabable();
                    break;
                default:
                    break;
            }
        }
    }
}
