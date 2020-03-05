using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeOutTriggerAI : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();

    private Scientist ai;
    [SerializeField] private float timeOut;
    [SerializeField] private float deltaStandStill;
    private float timer = 0;

    private void Start()
    {
        ai = FindObjectOfType<Scientist>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("velocity: " + rb.velocity.magnitude);
        if (rb.velocity.magnitude <= deltaStandStill)
        {
            
            timer += Time.deltaTime;
            if (timer >= timeOut)
            {
                Debug.Log("Scientist has been trigger");
                ai.Trigger(transform.position, null);
                timer = 0;
            }
        }
        else timer = 0;
    }
}
