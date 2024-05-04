using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField] GameObject pausemenu;
    public bool ispaused;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!ispaused)
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
        pausemenu.SetActive(true);
        Time.timeScale = 0f;
        ispaused = true;
    }
    public void resume()
    {
        pausemenu.SetActive(false);
        Time.timeScale = 1f;
        ispaused = false;
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
