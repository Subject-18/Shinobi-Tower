using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    public static bool ispaused = false;
    public GameObject PauseMenuCanvas;

    public  void Start()
    {
        Time.timeScale = 1.0f;
    }
    
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(ispaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }
    
    public void pause()
    {
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        ispaused = true;
        Cursor.visible = true;
    }
    public void resume()
    {
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        ispaused = false;
        Cursor.visible = false;
    }
    public void quitgame()
    {
        Application.Quit();
    }
    public void mainmenu()
    {
        SceneManager.LoadScene(0);
    }
}
