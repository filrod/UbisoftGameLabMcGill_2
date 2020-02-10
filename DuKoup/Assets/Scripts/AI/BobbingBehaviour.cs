using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range; // X and Y

    private Vector3 originalPos;
    private Vector3 moveTo;
    private bool movingFromCenter = true;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        moveTo = GenerateNewTarget();
    }

    // Update is called once per frame
    void Update()
    {

        if (movingFromCenter)
        {
            if (transform.position.Equals(moveTo))
            {
                movingFromCenter = false;
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, moveTo, speed);
        }
        else
        {
            if (transform.position.Equals(originalPos))
            {
                movingFromCenter = true;
                moveTo = GenerateNewTarget();
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, originalPos, speed);
        }
    }

    private Vector3 GenerateNewTarget()
    {
        float x = Random.Range(originalPos.x - range, originalPos.x + range);
        float y = Random.Range(originalPos.y, originalPos.y + range);

        return new Vector3(x, y, originalPos.z);
    }
}

