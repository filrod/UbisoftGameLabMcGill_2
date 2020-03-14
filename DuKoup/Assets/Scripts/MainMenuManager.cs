using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    // Main menu
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject localOnlineMenu;

    // Local and online menu
    [SerializeField] private GameObject localOnlineCanvas;
    [SerializeField] private GameObject localMenu;

    // bool to know in which menu the player is
    private bool isMainMenu = true;
    private bool isLocalOnline = false;
    private bool isLocal = false;

    [SerializeField] private GameObject waitingForPlay1;
    [SerializeField] private GameObject waitingForPlay2;

    [SerializeField] private GameObject pressXPlay1;
    [SerializeField] private GameObject pressXPlay2;

    [SerializeField] private GameObject eventSystem;

    [SerializeField] private GameObject localButton;
    [SerializeField] private GameObject startButton;

    private bool player1Connected = false;
    private bool player2Connected = false;

    public void Play()
    {
        mainMenu.SetActive(false);
        localOnlineMenu.SetActive(true);

        isMainMenu = false;
        isLocalOnline = true;
    }

    public void Settings()
    {
        Debug.Log("Entered settings");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Local()
    {
        isLocalOnline = false;
        isLocal = true;
        
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(localButton);

        mainMenuCanvas.SetActive(false);
        localOnlineCanvas.SetActive(true);
        localMenu.SetActive(true);

        waitingForPlay2.SetActive(false);
        pressXPlay1.SetActive(true);   

        waitingForPlay1.SetActive(false);
        pressXPlay2.SetActive(true);
    }

    public void Online()
    {
        Debug.Log("Enter creation room scene");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Back"))
        {
            Debug.Log("Input back");

            if (isMainMenu) return;

            else if (isLocalOnline)
            {
                isLocalOnline = false;
                isMainMenu = true;

                mainMenu.SetActive(true);
                localOnlineMenu.SetActive(false);
            }

            else if (isLocal)
            {
                isLocalOnline = true;
                isLocal = false;

                eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(startButton);

                mainMenuCanvas.SetActive(true);
                localOnlineCanvas.SetActive(false);

                localMenu.SetActive(true);
                mainMenu.SetActive(false);
                localMenu.SetActive(false);

                player1Connected = false;
                player2Connected = false;
            }
        }

        else if (isLocal)
            {
                if (Input.GetButtonDown("Jump1"))
                {
                    player1Connected = true;
                    waitingForPlay2.SetActive(true);
                    pressXPlay1.SetActive(false);
                }

                if (Input.GetButtonDown("Jump2"))
                {
                    player2Connected = true;
                    waitingForPlay1.SetActive(true);
                    pressXPlay2.SetActive(false);
                }

                if (player1Connected && player2Connected)
                {
                    SceneManager.LoadScene("Level1");
                }
            }
        
    }

}
