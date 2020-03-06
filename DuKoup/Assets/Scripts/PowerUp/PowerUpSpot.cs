using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpot : MonoBehaviour
{
    /// <summary>
    /// The player who enter the power up spot
    /// </summary>
    private GameObject enteredPlayer;

    public PowerUpInfo powerUpInfo;

    public void OnCollisionEnter(Collision collision)
    {
        // TODO: refactor the dummy, and unify the tag
        Debug.Log("Hit Power up spot");
        if (collision.gameObject.CompareTag("Player"))
        {
            enteredPlayer = collision.gameObject;
            gameObject.SetActive(false);
            // player power up
            PowerUp powerUp = enteredPlayer.GetComponent<PowerUp>();
            if (powerUp != null)
            {
                switch (powerUpInfo.CURRENT_TYPE)
                {
                    case PowerUpInfo.POWER_TYPE.Default:
                        powerUp.Reset();
                        break;
                    case PowerUpInfo.POWER_TYPE.Double_Jump:
                        powerUp.GainDoubleJump();
                        break;
                    case PowerUpInfo.POWER_TYPE.Grabable:
                        powerUp.GainGrabable();
                        break;
                    case PowerUpInfo.POWER_TYPE.High_Jump:
                        powerUp.GainHighJump();
                        break;
                    default:
                        break;
                }
            } else
            {
                Debug.LogWarning("Missing Power up component!");
            }
        }
    }
}
