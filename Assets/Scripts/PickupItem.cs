using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private bool canPickup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canPickup && Input.GetButtonDown(Global.Inputs.Fire1) && PlayerController.instance.canMove)
        {
            GameManager.instance.AddItem(GetComponent<Item>().itemName);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Global.Labels.PlayerTag)
        {
            canPickup = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Global.Labels.PlayerTag)
        {
            canPickup = false;
        }
    }
}
