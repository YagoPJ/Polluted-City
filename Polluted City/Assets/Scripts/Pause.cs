using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // timescale 0 pausa o jogo
        }
    }

    public void ResumeMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // 1 ele volta o jogo ao normal
        Cursor.visible = false;
    }

    public void RetornarMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}
