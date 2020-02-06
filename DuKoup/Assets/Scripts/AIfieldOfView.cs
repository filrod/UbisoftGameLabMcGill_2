using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIfieldOfView : MonoBehaviour
{
    private Vector2 wanderingTowards;
    private Vector2 targetPos = Vector2.zero;
    private bool isWandering = true;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;


    // Start is called before the first frame update
    void Start()
    {
        wanderingTowards = generateRandomWnaderingPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) PlayerSetPos();

        if (transform.position.Equals(targetPos)) targetPos = Vector2.zero;
        if (targetPos.Equals(Vector2.zero))
        {
            if (transform.position.Equals(wanderingTowards)) wanderingTowards = generateRandomWnaderingPos();
            if (isWandering)
            {
                transform.position = Vector3.MoveTowards(transform.position, wanderingTowards, 0.05f);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.08f);
        }
    }

    private void Wandering()
    {

    }

    private void PlayerSetPos()
    {
        Debug.Log("mouse click");
        targetPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    private Vector2 generateRandomWnaderingPos()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        Debug.Log(x + ", " + y);

        return new Vector2(x, y);
    }
}
