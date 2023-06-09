using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    public string questToCheck;

    public bool activeIfComplete;

    private bool initialCheckDone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialCheckDone)
        {
            initialCheckDone = true;
            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if (QuestManager.instance.CheckIsComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }
        else
        {
            objectToActivate.SetActive(false);
        }
    }
}
