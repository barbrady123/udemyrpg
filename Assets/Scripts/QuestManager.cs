using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.TextCore.Text;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public string[] questMarkerNames;
    public bool[] questMarkersComplete;

    public static QuestManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        questMarkersComplete = new bool[questMarkerNames.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetQuestIndex(string name)
    {
        int index = Array.IndexOf(questMarkerNames, name);
        if (index < 0)
        {
            // should error
            Debug.Log($"Encountered unknown quest name '{name}'");
        }

        return index;
    }

    public bool CheckIsComplete(string questToCheck)
    {
        int questIndex = GetQuestIndex(questToCheck);
        if (questIndex < 0)
            return false;

        return questMarkersComplete[questIndex];
    }

    public void MarkQuestComplete(string questToCheck, bool complete = true)
    {
        int questIndex = GetQuestIndex(questToCheck);
        if (questIndex < 0)
            return;

        questMarkersComplete[questIndex] = complete;
        UpdateLocalQuestObjects();
    }

    public void UpdateLocalQuestObjects()
    {
        // Apparently this scans the entire scene for objects with this script...
        foreach (var activator in FindObjectsOfType<QuestObjectActivator>())
        {
            activator.CheckCompletion();
        }
    }
}
