using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string newGameScene;

    public GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        continueButton.SetActive(File.Exists(GameManager.SaveGameLoc));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Continue()
    {
        SceneManager.LoadScene("Init");
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
