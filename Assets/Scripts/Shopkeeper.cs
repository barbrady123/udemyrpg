using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    private bool canOpen;

    public string[] itemsForSale = new string[40];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetButtonDown(Global.Inputs.Fire1) && PlayerController.instance.canMove)
        {
            Shop.instance.itemsForSale = itemsForSale; 
            Shop.instance.OpenShop();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Global.Labels.PlayerTag)
        {
            canOpen = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Global.Labels.PlayerTag)
        {
            canOpen = false;
        }
    }
}
