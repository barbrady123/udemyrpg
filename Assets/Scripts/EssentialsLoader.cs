using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;

    // Awake is called when the script is first loaded (prior to Start)
    private void Awake()
    {
        if (UIFade.instance == null)
        {
            Instantiate(UIScreen);
        }

        if (PlayerController.instance == null)
        {
            Instantiate(player);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
