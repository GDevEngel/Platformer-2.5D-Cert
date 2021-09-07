using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // singleton 

    private static SceneLoader _instance;
    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null) { Debug.LogError("Scene Manager _instance is null"); }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    //esc to exit
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadGame();
        }
    }

    public void LoadGame()
    {
        Debug.Log("try load scene 1");
        SceneManager.LoadScene(1);
    }

}
