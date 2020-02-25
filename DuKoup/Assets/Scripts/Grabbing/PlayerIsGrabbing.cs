using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartCoroutine("StopGrabbing");
    }

    IEnumerator StopGrabbing()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("isGrabbing", false);
    }
}
