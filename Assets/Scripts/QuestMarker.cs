using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour
{
    public string questToMark;

    public bool markComplete;
    public bool markOnEnter;

    public bool singleUse = true;

    private bool canMark;

    // Start is called before the first frame update
    void Start()
    {
        if (QuestInMarkedState() && singleUse)
        {
            gameObject.SetActive(false);
        }
    }

    private bool QuestInMarkedState() => QuestManager.instance.CheckIsComplete(questToMark) == markComplete;

    private void MarkQuest()
    {
        QuestManager.instance.MarkQuestComplete(questToMark, markComplete);
        if (singleUse)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMark && Input.GetButtonDown(Global.Inputs.Fire1))
        {
            MarkQuest();
            canMark = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Global.Labels.PlayerTag)
        {
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Global.Labels.PlayerTag)
        {
            canMark = false;
        }
    }
}
