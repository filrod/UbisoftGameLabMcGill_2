using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : IButtonBehaviour
{
    override public void ButtonDown()
    {
       SceneManager.LoadScene("Level2");
    }

    override public void ButtonUp()
    {
        Debug.Log("Button pushed up");
    }
}
