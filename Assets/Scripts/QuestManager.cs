using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // DON'T USE PlayerPrefs FOR THIS....this is dumb...
    public void SaveQuestData()
    {
        foreach ((string questName, int x) in questMarkerNames.WithIndex())
        {
            PlayerPrefs.SetInt($"QuestMarker_{questName}", questMarkersComplete[x] ? 1 : 0);
        }
    }

    public void LoadQuestData()
    {
        foreach ((string questName, int x) in questMarkerNames.WithIndex())
        {
            questMarkersComplete[x] = (PlayerPrefs.GetInt($"QuestMarker_{questName}", 0) == 1);
        }
    }
}
