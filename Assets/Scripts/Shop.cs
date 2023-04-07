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

    public Item selectedItem;

    public Text buyItemName;
    public Text buyItemDescription;
    public Text buyItemValue;

    public Text sellItemName;
    public Text sellItemDescription;
    public Text sellItemValue;

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

        SelectBuyItem();

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

        SelectSellItem();

        RefreshSellMenu();
    }

    public void SelectBuyItem(Item buyItem = null)
    {
        selectedItem = buyItem;

        if (selectedItem != null)
        {
            var itemInfo = GameManager.instance.GetItemDetails(selectedItem.itemName);
            buyItemName.text = selectedItem.itemName;
            buyItemDescription.text = itemInfo.description;
            buyItemValue.text = $"Cost: {itemInfo.value}g";
        }
        else
        {
            buyItemName.text = null;
            buyItemDescription.text = null;
            buyItemValue.text = null;
        }
    }

    public void SelectSellItem(Item sellItem = null)
    {
        selectedItem = sellItem;

        if (selectedItem != null)
        {
            var itemInfo = GameManager.instance.GetItemDetails(selectedItem.itemName);
            sellItemName.text = selectedItem.itemName;
            sellItemDescription.text = itemInfo.description;
            sellItemValue.text = $"Value: {itemInfo.value}g";
        }
        else
        {
            sellItemName.text = null;
            sellItemDescription.text = null;
            sellItemValue.text = null;
        }
    }

    public void BuySelectedItem()
    {
        if (selectedItem == null)
            return;

        if (selectedItem.value > GameManager.instance.currentGold)
        {
            Debug.Log("YOU DONT'T GOT ENOUGH MONEY!");
            return;
        }

        GameManager.instance.currentGold -= selectedItem.value;
        GameManager.instance.AddItem(selectedItem.itemName, 1);
        goldText.text = GameManager.instance.currentGold.ToString();
    }

    public void SellSelectedItem()
    {
        if (selectedItem == null)
            return;

        GameManager.instance.currentGold += selectedItem.value;

        if (GameManager.instance.RemoveItem(selectedItem.itemName, 1))
        {
            SelectSellItem();
        }

        goldText.text = GameManager.instance.currentGold.ToString();
        RefreshSellMenu();
    }

    public void RefreshSellMenu()
    {
        GameManager.instance.SortItems();

        for (int x = 0; x < sellItemButtons.Length; x++)
        {
            sellItemButtons[x].buttonValue = x;

            if (GameManager.instance.itemsHeld[x] != "")
            {
                sellItemButtons[x].buttonImage.gameObject.SetActive(true);
                sellItemButtons[x].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[x]).itemSprite;
                sellItemButtons[x].amountText.text = GameManager.instance.numberOfItems[x].ToString();
            }
            else
            {
                sellItemButtons[x].buttonImage.gameObject.SetActive(false);
                sellItemButtons[x].amountText.text = "";
            }
        }
    }
}
