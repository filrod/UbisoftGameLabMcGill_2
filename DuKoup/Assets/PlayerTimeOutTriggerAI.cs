using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeOutTriggerAI : MonoBehaviour
{
    private Rigidbody rb => transform.GetChild(0).GetComponent<Rigidbody>();

    [SerializeField] private Scientist ai;
    [SerializeField] private float timeOut;
    [SerializeField] private float deltaStandStill;
    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude <= deltaStandStill)
        {
            timer += Time.deltaTime;
            if (timer >= timeOut)
            {
                ai.Trigger(transform.GetChild(0).transform.position, null);
                timer = 0;
            }
        }
        else timer = 0;
    }
}
