using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public string[] lines;

    private bool canActivate;

    private NPCController activatingNPC;

    // Start is called before the first frame update
    void Start()
    {
        activatingNPC = gameObject.GetComponent<NPCController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canActivate && Input.GetButtonDown(Global.Inputs.Fire1) && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines, activatingNPC?.npcName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Global.Labels.PlayerTag)
        {
            canActivate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Global.Labels.PlayerTag)
        {
            canActivate = false;
        }
    }
}
