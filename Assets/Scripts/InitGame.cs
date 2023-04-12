using UnityEngine;

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

        GameManager.instance.gameOver = false;
        GameManager.instance.LoadGame();
    }
}
