using System.Collections;
using UnityEngine;


/// <summary>
/// UBISOFT GAMES LAB - McGill Team #2
/// -----------------------------------
/// @author Rikke Aas
/// @Date 2020/02/28
///
/// This class tells us wether the player has pressed the grab button and thus we should play the grab animation
/// </summary>
public class PlayerIsGrabbing : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int playerId;


    private void Update()
    {
        if (playerId == 1)
        {
            if (Input.GetButtonDown("Grab1")) StartGrabbing();
        }

        else if (playerId == 2)
        {
            if (Input.GetButtonDown("Grab2")) StartGrabbing();
        }
    }



    public void StartGrabbing()
    {
        animator.SetBool("isGrabbing", true);
        StartCoroutine("StopGrabbing"); // Starting a parallell process to stop grabbing after 1 second
    }

    /// <summary>
    /// Corutine to wait for a second (letting the grab animation play) before setting the isGrabbing state back to false
    /// </summary>
    /// <returns></returns>
    IEnumerator StopGrabbing()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("isGrabbing", false);
    }
}
