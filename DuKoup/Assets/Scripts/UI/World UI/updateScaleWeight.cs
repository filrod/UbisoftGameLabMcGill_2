using UnityEngine;

/// <summary>
/// Ubisoft Game Labs
/// ------------------
/// 
/// @author Filipe
/// @Date 2020/03/06
/// 
/// Updates number on scales while a vile or player 
/// collide with it.
/// </summary>
public class updateScaleWeight : MonoBehaviour
{
    /// <summary>
    /// A counter for total mass on this scale.
    /// </summary>
    private float mass = 0;
    /// <summary>
    /// The text on the scale
    /// </summary>
    private TextMesh txt;

    /// <summary>
    /// Set the text component to the TextMesh component (child of scale object)
    /// </summary>
    private void Start()
    {
        txt = GetComponentInChildren<TextMesh>();
    }

    /// <summary>
    /// Update the mass reading on the scale
    /// </summary>
    private void Update()
    {
        txt.text = mass.ToString("0.00")+"g";
    }

    /// <summary>
    /// Adds player of vyle's mass when it collides anywhere with the scale's collider on the main object
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision == null) return;
        if ((collision.gameObject.name.Split(' ')[0] == "Player") || (collision.gameObject.name.Split(' ')[0] == "bottle_04_wide"))
            mass += collision.gameObject.GetComponent<Rigidbody>().mass;
        else
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
    }
    /// <summary>
    /// Adds player of vyle's mass when it collides anywhere with the scale's collider on the main object
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        //if (collision == null) return;
        if ( (collision.gameObject.name.Split(' ')[0] == "Player") || (collision.gameObject.name.Split(' ')[0] == "bottle_04_wide") )
            mass -= collision.gameObject.GetComponent<Rigidbody>().mass;
        else
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
    }
}
