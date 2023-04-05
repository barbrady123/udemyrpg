using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharStats : MonoBehaviour
{
	public string charName = Global.Labels.DefaultCharacterName;

    public int playerLevel = 1;
    public int currentXP;
    public int[] xpToNextLevel;
    public int maxLevel = 100;
    public int baseXP = 1000;

    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;

    public int strength;
    public int defense;

    public int weaponPower;
    public int armorPower;

    public string equippedWeapon;
    public string equippedArmor;

    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
        xpToNextLevel = new int[maxLevel + 1];
        xpToNextLevel[0] = 0;
        int lastLevel = 0;

        for (int i = 1; i <= maxLevel; i++)
        {
            int nextXP = Convert.ToInt32(baseXP * Math.Pow(1.05, i - 1));
            lastLevel = xpToNextLevel[i] = lastLevel + nextXP;
        }

        xpToNextLevel[maxLevel] = Int32.MaxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddXP(500);
        }
    }

    public void AddXP(int xp)
    {
        currentXP += xp;

        int currentLevel = xpToNextLevel.Select((value, index) => new { value, index }).First(x => x.value > currentXP).index;

        while (currentLevel > playerLevel)
        {
            playerLevel++;

            maxHP = Convert.ToInt32(maxHP * 1.05f);
            currentHP = maxHP;

            maxMP += 5;
            currentMP = maxMP;

            if (playerLevel % 2 == 0)
            {
                strength++;
            }
            else
            {
                defense++;
            }
        }
    }

    public int AddHP(int amount)
    {
        currentHP = Math.Clamp(currentHP + amount, 0, maxHP);
        return currentHP;
    }

    public int AddMP(int amount)
    {
        currentMP = Math.Clamp(currentMP + amount, 0, maxMP);
        return currentMP;
    }

    public string EquipWeapon(string weapon)
    {
        string current = equippedWeapon;
        if (!String.IsNullOrWhiteSpace(current))
        {
            var currentDetails = GameManager.instance.GetItemDetails(current);
            weaponPower = Math.Max(weaponPower - currentDetails.weaponStrength, 0);
        }

        equippedWeapon = weapon;
        var newDetails = GameManager.instance.GetItemDetails(weapon);
        weaponPower += newDetails.weaponStrength;

        return current;
    }

    public string EquipArmor(string armor)
    {
        string current = equippedArmor;
        if (!String.IsNullOrWhiteSpace(current))
        {
            var currentDetails = GameManager.instance.GetItemDetails(current);
            armorPower = Math.Max(armorPower - currentDetails.armorStrength, 0);
        }

        equippedArmor = armor;
        var newDetails = GameManager.instance.GetItemDetails(armor);
        armorPower += newDetails.armorStrength;

        return current;
    }
}
