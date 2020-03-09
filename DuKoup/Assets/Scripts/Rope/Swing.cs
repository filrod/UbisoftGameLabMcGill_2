using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField]
    public Pendulum pendulum;

    // Start is called before the first frame update
    void Start()
    {
        pendulum.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = pendulum.MoveBob(transform.localPosition, Time.deltaTime);
    }
}
