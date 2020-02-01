using UnityEngine;

/**
 * UBISOFT GAMES LAB - McGill Team #2
 * -----------------------------------
 * 
 * @author Rikke Aas
 * @Date 2020/01/31
 * 
 * This class controls the behaviour for the planking space prefab, specifically the turning on and off of the "bridge"
*/

public class PlankingSpace : MonoBehaviour
{

    public void TurnOnBridge()
    {
        // Child 0 is the "bridge" (gameobject with only a collider attached)
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void TurnOffBridge()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

}
