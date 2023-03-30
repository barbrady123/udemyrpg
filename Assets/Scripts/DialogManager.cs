using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogText;
    public GameObject nameBox;
    public GameObject dialogBox;

    public string[] dialogLines;

    public int currentLine;

    public static DialogManager instance;
    private bool justStarted;

    public string currentName;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetButtonUp(Global.Inputs.Fire1))
            {
                if (!justStarted)
                {
                    currentLine++;

                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                        PlayerController.instance.canMove = true;
                    }
                    else
                    {
                        SetText(dialogLines[currentLine]);
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }
    }

    public void ShowDialog(string[] newLines, string name)
    {
        dialogLines = newLines;
        currentName = name;

        currentLine = 0;

        SetText(dialogLines[currentLine], true);
        dialogBox.SetActive(true);

        justStarted = true;

        PlayerController.instance.canMove = false;
    }

    private void SetText(string line, bool resetSpeaker = false)
    {
        if (resetSpeaker)
        {
            nameText.color = GetColor();
            nameText.text = GetName();
        }

        var parts = line.Split(':');

        bool showName = true;
        
        if (parts.Length > 1)
        {
            showName = parts[0] != Global.Labels.NoChat;
            nameBox.SetActive(showName);

            nameText.color = GetColor(parts[0]);
            nameText.text = GetName(parts[0]);
        }

        dialogText.text = parts.Last();
        dialogText.alignment = showName ? TextAnchor.UpperLeft : TextAnchor.MiddleCenter;
    }

    private Color GetColor(string token = null)
    {
        return token switch
        {
            Global.Labels.NPCChat => Global.Colors.NPCName,
            Global.Labels.PlayerChat => Global.Colors.PlayerName,
            _ => Global.Colors.Default,
        };
    }

    private string GetName(string token = null)
    {
        return token switch
        {
            Global.Labels.NPCChat => currentName,
            Global.Labels.PlayerChat => PlayerController.instance.playerName,
            null => Global.Labels.DefaultNPCDisplay,
            _ => token,
        };
    }
}
