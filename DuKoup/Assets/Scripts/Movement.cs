using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Filipe Rodrigues
 * @Date 2020/01/28
 * 
 * This class controls player movement
*/
public class Movement : MonoBehaviour
{
    public Rigidbody player;
    public float speed = 0.2f;

    float horizontalMovement;
    Vector3 movement;


    // Start is called before the first frame update
    void Start()
    {
        horizontalMovement = 0;
        movement = new Vector3(horizontalMovement, 0f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement += Input.GetAxis("Horizontal");
        movement = new Vector3(horizontalMovement * speed, 0f, 0f);
        player.transform.position += movement;
    }
}
