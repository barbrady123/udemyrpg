using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour
{
    private string _spellName;
    private int _spellCost;
    public Text nameText;
    public Text costText;

    public string spellName
    {
        get => _spellName;
        set
        {
            _spellName = value;
            if (nameText != null) nameText.text = _spellName;
        }
    }

    public int spellCost
    {
        get => _spellCost;
        set
        {
            _spellCost = value;
            if (costText != null) costText.text = _spellCost.ToString();
        }
    }

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
        if (BattleManager.instance.ActiveBattler.currentMP < spellCost)
        {
            BattleManager.instance.notification.Activate("Not Enough Mana!", 1);
            return;
        }

        BattleManager.instance.magicMenu.SetActive(false);
        BattleManager.instance.OpenTargetMenu(spellName);
        BattleManager.instance.ActiveBattler.currentMP -= spellCost;
    }
}
