using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    public float waitToLoad;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;
            return;
        }

        GameManager.instance.LoadGame();
    }
}
