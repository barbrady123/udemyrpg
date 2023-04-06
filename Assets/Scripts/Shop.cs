using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    public Text goldText;   

    public string[] itemsForSale;

    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        GameManager.instance.shopActive = true;
        goldText.text = GameManager.instance.currentGold.ToString();
        OpenBuyMenu();
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        GameManager.instance.shopActive = false;
    }

    public void OpenBuyMenu()
    {
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for (int x = 0; x < buyItemButtons.Length; x++)
        {
            buyItemButtons[x].buttonValue = x;

            if (itemsForSale[x] != "")
            {
                buyItemButtons[x].buttonImage.gameObject.SetActive(true);
                buyItemButtons[x].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[x]).itemSprite;
                buyItemButtons[x].amountText.text = "";
            }
            else
            {
                buyItemButtons[x].buttonImage.gameObject.SetActive(false);
                buyItemButtons[x].amountText.text = "";
            }
        }
    }

    public void OpenSellMenu()
    {
        sellMenu.SetActive(true);
        buyMenu.SetActive(false);
    }
}
