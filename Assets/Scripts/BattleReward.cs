using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleReward : MonoBehaviour
{
    public static BattleReward instance;

    public Text xpText;
    public Text itemText;
    public GameObject rewardScreen;

    public string[] rewardItems;
    public int xpEarned;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRewardScreen(int xpEarned, string[] rewardItems)
    {
        this.xpEarned = xpEarned;
        this.rewardItems = rewardItems;

        xpText.text = $"Everyone earned {xpEarned} xp!";
        itemText.text = String.Join(Environment.NewLine, rewardItems);

        rewardScreen.SetActive(true);
    }

    public void CloseRewardScreen()
    {
        foreach (var stat in GameManager.instance.playerStats.Where(x => x.gameObject.activeInHierarchy))
        {
            stat.AddXP(xpEarned);            
        }

        foreach (var item in rewardItems)
        {
            GameManager.instance.AddItem(item);
        }
        
        rewardScreen.SetActive(false);
    }
}
