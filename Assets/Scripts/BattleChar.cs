using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    public bool IsPlayer;
    public string[] movesAvailable;

    public string charName;

    public int currentHP;
    public int maxHP;
    public int currentMP;
    public int maxMP;
    public int strength;
    public int defense;
    public int weaponPower;
    public int armorPower;

    public bool hasDied;

    public int TotalAttackPower => this.strength + this.weaponPower;
    public int TotalDefensePower => this.defense + this.armorPower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
