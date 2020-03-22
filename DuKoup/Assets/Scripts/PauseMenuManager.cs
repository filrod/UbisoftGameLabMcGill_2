using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private string currScene;

    private bool isPause = false;

    public void Continue()
    {
        Time.timeScale = 1f;
        pauseMenuCanvas.SetActive(false);
        isPause = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currScene);
    }

    public void Settings()
    {
        Debug.Log("Entered settings");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!isPause && Input.GetButtonDown("Cancel"))
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            isPause = true;
        }
    }
}
