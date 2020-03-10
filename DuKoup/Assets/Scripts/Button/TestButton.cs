
using UnityEngine;

public class TestButton : IButtonBehaviour
{
    override public void ButtonDown()
    {
        Debug.Log("Button pushed down");
    }

    override public void ButtonUp()
    {
        Debug.Log("Button pushed up");
    }
}
