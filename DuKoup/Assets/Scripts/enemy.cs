using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemy : MonoBehaviour
{
    /// <summary>
    /// something
    /// </summary>
    [Tooltip("Here is a tip")] [SerializeField] private Collider regionOfAttack;
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            player.SetActive(false);
            SceneManager.LoadScene("SampleScene");
        }
    }

    bool attack()
    {
        bool hit = false;

        // Attack

        // Check if hit

        return hit;
    }

}
