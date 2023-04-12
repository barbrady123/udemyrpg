using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;
    public GameObject[] windows;

    private CharStats[] playerStats;

    public GameObject[] charStatHolder;

    public Text[] nameText, hpText, mpText, lvlText, xpText;
    public Slider[] xpSlider;
    public Image[] charImage;

    public GameObject[] statusButtons;

    public Text statusName;
    public Text statusHP;
    public Text statusMP;
    public Text statusStr;
    public Text statusDef;
    public Text statusWeaponEq;
    public Text statusWeaponPower;
    public Text statusArmorEq;
    public Text statusArmorPower;
    public Text statusXP;
    public Image statusImage;

    public ItemButton[] itemButtons;

    public string selectedItem;
    public Item activeItem;
    public Text itemName;
    public Text itemDescription;
    public Text useButtonText;

    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public static GameMenu instance;
    public Text goldText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Global.Inputs.Fire2) && GameManager.instance.CanOpenMenu())
        {
            if (theMenu.activeInHierarchy)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;

        for (int x = 0; x < playerStats.Length; x++)
        {
            charStatHolder[x].SetActive(playerStats[x].gameObject.activeInHierarchy);

            if (playerStats[x].gameObject.activeInHierarchy)
            {
                nameText[x].text = playerStats[x].charName;
                hpText[x].text = $"{playerStats[x].currentHP} / {playerStats[x].maxHP}";
                mpText[x].text = $"{playerStats[x].currentMP} / {playerStats[x].maxMP}";
                lvlText[x].text = $"Lvl: {playerStats[x].playerLevel}";
                xpText[x].text = $"{playerStats[x].currentXP} / {playerStats[x].xpToNextLevel[playerStats[x].playerLevel]}";
                xpSlider[x].minValue = 0;
                xpSlider[x].maxValue = playerStats[x].xpToNextLevel[playerStats[x].playerLevel] - playerStats[x].xpToNextLevel[playerStats[x].playerLevel - 1];
                xpSlider[x].value = playerStats[x].currentXP - playerStats[x].xpToNextLevel[playerStats[x].playerLevel - 1];
                charImage[x].sprite = playerStats[x].charImage;
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString();
    }

    public void ToggleWindow(int windowNumber = -1)
    {
        UpdateMainStats();
        AudioManager.instance.PlaySFX(4);

        for (int x = 0; x < windows.Length; x++)
        {
            windows[x].SetActive(x == windowNumber ? !windows[x].activeInHierarchy : false);
        }

        CloseItemCharChoice();
    }

    public void OpenMenu()
    {
        AudioManager.instance.PlaySFX(5);
        UpdateMainStats();
        theMenu.SetActive(true);
        GameManager.instance.gameMenuOpen = true;
    }

    public void CloseMenu()
    {
        AudioManager.instance.PlaySFX(4);
        ToggleWindow();
        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;

        CloseItemCharChoice();
    }

    public void OpenStatus()
    {
        UpdateMainStats();

        // Update the information that is shown
        StatusChar(0);

        for (int x = 0; x < statusButtons.Length; x++)
        {
            statusButtons[x].SetActive(playerStats[x].gameObject.activeInHierarchy);
            statusButtons[x].GetComponentInChildren<Text>().text = playerStats[x].charName;
        }
    }

    public void StatusChar(int selected)
    {
        statusName.text = playerStats[selected].charName;
        statusHP.text = $"{playerStats[selected].currentHP} / {playerStats[selected].maxHP}";
        statusMP.text = $"{playerStats[selected].currentMP} / {playerStats[selected].maxMP}";
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defense.ToString();
        statusWeaponEq.text = String.IsNullOrWhiteSpace(playerStats[selected].equippedWeapon) ? Global.Labels.DefaultWeaponDisplay : playerStats[selected].equippedWeapon;
        statusWeaponPower.text = playerStats[selected].weaponPower.ToString();
        statusArmorEq.text = String.IsNullOrWhiteSpace(playerStats[selected].equippedArmor) ? Global.Labels.DefaultArmorDisplay : playerStats[selected].equippedArmor;
        statusArmorPower.text = playerStats[selected].armorPower.ToString();
        statusXP.text = $"{playerStats[selected].currentXP} / {playerStats[selected].xpToNextLevel[playerStats[selected].playerLevel]}";
        statusImage.sprite = playerStats[selected].charImage;
    }

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for (int x = 0; x < itemButtons.Length; x++)
        {
            itemButtons[x].buttonValue = x;

            if (GameManager.instance.itemsHeld[x] != "")
            {
                itemButtons[x].buttonImage.gameObject.SetActive(true);
                itemButtons[x].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[x]).itemSprite;
                itemButtons[x].amountText.text = GameManager.instance.numberOfItems[x].ToString();
            }
            else
            {
                itemButtons[x].buttonImage.gameObject.SetActive(false);
                itemButtons[x].amountText.text = "";
            }
        }

        SelectItem(activeItem);
    }

    public void SelectItem(Item newItem = null)
    {
        activeItem = newItem;

        if (newItem != null)
        {
            useButtonText.text = (activeItem.isWeapon || activeItem.isArmor) ? "Equip" : "Use";

            itemName.text = activeItem.itemName;
            itemDescription.text = activeItem.description;
        }
        else
        {
            itemName.text = null;
            itemDescription.text = null;
        }
    }

    public void DiscardActiveItem()
    {
        if (activeItem == null)
            return;

        if (GameManager.instance.RemoveItem(activeItem.itemName))
        {
            activeItem = null;
        }

        SelectItem(activeItem);
    }

    public void OpenItemCharChoice()
    {
        if (activeItem == null)
            return;

        itemCharChoiceMenu.SetActive(true);

        for (int x = 0; x < itemCharChoiceNames.Length; x++)
        {
            itemCharChoiceNames[x].text = GameManager.instance.playerStats[x].charName;
            itemCharChoiceNames[x].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[x].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectedChar)
    {
        activeItem?.Use(selectedChar);
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
    }

    public void QuitGame(bool saveGame = true)
    {
        if (saveGame)
        {
            SaveGame();
        }

        SceneManager.LoadScene(Global.Scenes.MainMenu);
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
