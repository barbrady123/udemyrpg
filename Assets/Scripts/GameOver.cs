using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameOver = true;
        AudioManager.instance.PlayBGM(Global.Music.Title);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuitToMain()
    {
        // Makes no sense to call the GameMenu for this...
        GameMenu.instance.QuitGame(false);
    }

    public void LoadLastSave()
    {
        SceneManager.LoadScene(Global.Scenes.Init);
    }
}
