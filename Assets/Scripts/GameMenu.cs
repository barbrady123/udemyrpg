using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;

    private CharStats[] playerStats;

    public GameObject[] charStatHolder;

    public Text[] nameText, hpText, mpText, lvlText, xpText;
    public Slider[] xpSlider;
    public Image[] charImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Global.Inputs.Fire2) && GameManager.instance.CanOpenMenu())
        {
            UpdateMainStats();
            theMenu.SetActive(!theMenu.activeInHierarchy);
            GameManager.instance.gameMenuOpen = theMenu.activeInHierarchy;
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
    }
}
