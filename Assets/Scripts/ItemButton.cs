using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public Text amountText;
    public int buttonValue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Press()
    {
        switch (GetComponentInParent<ItemButtonContainer>().buttonType)
        {
            case ItemButtonContainerType.PlayerItems:
                if (GameManager.instance.itemsHeld[buttonValue] != "")
                {
                    GameMenu.instance.SelectItem(
                        GameManager.instance.GetItemDetails(
                            GameManager.instance.itemsHeld[buttonValue]));
                }
                break;
            case ItemButtonContainerType.ShopBuyItems:
                Shop.instance.SelectBuyItem(
                    GameManager.instance.GetItemDetails(
                        Shop.instance.itemsForSale[buttonValue]));
                break;
            case ItemButtonContainerType.ShopSellItems:
                Shop.instance.SelectSellItem(
                    GameManager.instance.GetItemDetails(
                        GameManager.instance.itemsHeld[buttonValue]));
                break;
        }

    }
}
