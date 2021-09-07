using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    //esc to exit
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadMenu();
        }
    }

    public void LoadMenu()
    {
        Debug.Log("try load scene 0");
        SceneManager.LoadScene(0);
    }
}
