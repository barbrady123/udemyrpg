using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Details")]
    public int amountToChange;
    public bool affectHP;
    public bool affectMP;
    public bool affectStr;

    [Header("Weapon/Armor Details")]
    public int weaponStrength;
    public int armorStrength;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use(int charToUseOn)
    {
        var selectedChar = GameManager.instance.playerStats[charToUseOn];
        GameMenu.instance.DiscardActiveItem();
        GameManager.instance.SortItems();

        string previous = null;

        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.AddHP(amountToChange);
            }

            if (affectMP)
            {
                selectedChar.AddMP(amountToChange);
            }

            if (affectStr)
            {
                selectedChar.strength += amountToChange;
            }
        }
        else if (isWeapon)
        {
            previous = selectedChar.EquipWeapon(itemName);
        }
        else if (isArmor)
        {
            previous = selectedChar.EquipArmor(itemName);
        }

        if (!String.IsNullOrWhiteSpace(previous))
        {
            GameManager.instance.AddItem(previous);
        }
    }
}
