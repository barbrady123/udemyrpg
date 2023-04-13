using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers;

    public int currentTurn;
    public bool turnWaiting;

    public GameObject uiButtonsHolder;

    public BattleMove[] movesList;

    public GameObject enemyAttackEffect;

    public DamageNumber damageNumber;

    public Text[] playerNames;
    public Text[] playerHP;
    public Text[] playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public BattleNotification notification;

    public int chanceToFlee = 35;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public BattleChar ActiveBattler => (battleActive && (currentTurn < (activeBattlers?.Count ?? 0))) ? activeBattlers[currentTurn] : null;

    public BattleMove GetMove(string name) => movesList?.FirstOrDefault(x => x.moveName == name);

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new[] { "Eyeball", "Spider", "Skeleton" });
        }

        if (!battleActive)
            return;

        if (turnWaiting)
        {
            uiButtonsHolder.SetActive(ActiveBattler.IsPlayer);

            if (ActiveBattler.IsPlayer)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    NextTurn();
                }
            }
            else
            {
                // enemy attack...
                StartCoroutine(EnemyMoveCo());
            }
        }
    }

    private void ResetActiveBattlers()
    {
        foreach (var battler in activeBattlers ?? Enumerable.Empty<BattleChar>())
        {
            Destroy(battler.gameObject);
        }

        activeBattlers = new List<BattleChar>();

        foreach (var pos in playerPositions)
        {
            while (pos.childCount > 0)
            {
                DestroyImmediate(pos.GetChild(0).gameObject);
            }
        }

        foreach (var pos in enemyPositions)
        {
            while (pos.childCount > 0)
            {
                DestroyImmediate(pos.GetChild(0).gameObject);
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (battleActive)
            return;

        battleActive = true;
        GameManager.instance.battleActive = true;
        ResetActiveBattlers();
        targetMenu.SetActive(false);

        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        battleScene.SetActive(true);

        AudioManager.instance.PlayBGM(6);

        turnWaiting = true;
        currentTurn = 0;    // Random.Range(0, activeBattlers.Count);

        foreach ((var playerPos, int index) in playerPositions.WithIndex())
        {

            if (!GameManager.instance.playerStats[index].gameObject.activeInHierarchy)
                continue;

            var stats = GameManager.instance.playerStats[index];
            var prefab = playerPrefabs.FirstOrDefault(x => x.charName == stats.charName);
            if (prefab == null)
            {
                Debug.LogError($"Unable to find prefab for character name '{stats.charName}' (Pos: {index})");
                continue;
            }

            var newPlayer = Instantiate(prefab, playerPos.position, playerPos.rotation);
            newPlayer.transform.parent = playerPos;

            newPlayer.IsPlayer = true;
            newPlayer.charName = stats.charName;
            newPlayer.currentHP = stats.currentHP;
            newPlayer.maxHP = stats.maxHP;
            newPlayer.currentMP = stats.currentMP;
            newPlayer.maxMP = stats.maxMP;
            newPlayer.strength = stats.strength;
            newPlayer.defense = stats.defense;
            newPlayer.weaponPower = stats.weaponPower;
            newPlayer.armorPower = stats.armorPower;
            newPlayer.hasDied = newPlayer.currentHP <= 0;

            activeBattlers.Add(newPlayer);
        }

        foreach ((var enemyPos, int index) in enemyPositions.WithIndex())
        {
            if (index >= enemiesToSpawn.Length)
                break;

            var prefab = enemyPrefabs.FirstOrDefault(x => x.charName == enemiesToSpawn[index]);
            if (prefab == null)
            {
                Debug.LogError($"Unable to find prefab for character name '{enemiesToSpawn[index]}' (Pos: {index})");
                continue;
            }

            var newEnemy = Instantiate(prefab, enemyPos.position, enemyPos.rotation);
            newEnemy.transform.parent = enemyPos;

            newEnemy.IsPlayer = false;
            newEnemy.charName = prefab.charName;
            newEnemy.currentHP = prefab.currentHP;
            newEnemy.maxHP = prefab.maxHP;
            newEnemy.currentMP = prefab.currentMP;
            newEnemy.maxMP = prefab.maxMP;
            newEnemy.strength = prefab.strength;
            newEnemy.defense = prefab.defense;
            newEnemy.weaponPower = prefab.weaponPower;
            newEnemy.armorPower = prefab.armorPower;
            newEnemy.hasDied = prefab.currentHP <= 0;

            activeBattlers.Add(newEnemy);
        }

        UpdateUIStats();
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;
        UpdateBattle();
    }

    public void UpdateBattle()
    {
        var players = activeBattlers.AllPlayers().ToArray();
        var enemies = activeBattlers.AllEnemies().ToArray();

        foreach (var player in players)
        {
            if (player.currentHP <= 0)
            {
                if (!player.hasDied)
                {
                    // Some indicator of death...
                    player.hasDied = true;
                }

                player.theSprite.sprite = player.deadSprite;
            }
            else
            {
                player.theSprite.sprite = player.aliveSprite;
            }
        }

        foreach (var enemy in enemies.Where(x => (x.currentHP <= 0) && (!x.hasDied)))
        {
            // Do something to indicate death...
            enemy.EnemyFade();
            enemy.hasDied = true;
        }

        UpdateUIStats();

        bool allDead = false;
        CombatEndCondition condition = CombatEndCondition.PlayerDeath;

        if (enemies.AllDead())
        {
            // Victory...
            condition = CombatEndCondition.PlayerVictory;
            allDead = true;
        }
        else if (players.AllDead())
        {
            // Defeat...
            allDead = true;
        }

        if (allDead)
        {
            battleActive = false;
            StartCoroutine(ExitBattleCo(condition));
        }
        else while (ActiveBattler.hasDied)
        {
            NextTurn();
        }
    }

    public IEnumerator ExitBattleCo(CombatEndCondition condition)
    {
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);

        var players = activeBattlers.AllPlayers().ToArray();
        var enemies = activeBattlers.AllEnemies().ToArray();
        int totalXP = enemies.Sum(x => 10 /*x.XPValue*/);
        int xpPerPlayer = 0;

        foreach (var battler in players)
        {
            var player = GameManager.instance.playerStats.Single(x => x.charName == battler.charName);
            player.currentHP = battler.currentHP;
            player.currentMP = battler.currentMP;

            if (condition == CombatEndCondition.PlayerVictory)
            {
                xpPerPlayer = totalXP / players.Length;
            }
        }

        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(2);
        battleScene.SetActive(false);
        ResetActiveBattlers();

        if (condition == CombatEndCondition.PlayerDeath)
        {
            SceneManager.LoadScene(Global.Scenes.GameOver);
        }

        UIFade.instance.FadeFromBlack();
        GameManager.instance.battleActive = false;
        CameraController.instance.PlayMusic();

        if (condition == CombatEndCondition.PlayerVictory)
        {
            BattleReward.instance.OpenRewardScreen(xpPerPlayer, GenerateLoot());
        }
    }

    private string[] GenerateLoot()
    {
        float currentChanceOfItem = 100.0f;
        float additionalChanceModifier = 0.4f;

        bool finishedLoot = false;
        var loot = new List<string>();


        while (!finishedLoot)
        {
            if (Random.Range(0, 100f) < currentChanceOfItem)
            {
                loot.Add(RandomItem());
                currentChanceOfItem *= additionalChanceModifier;
                continue;
            }

            finishedLoot = true;                
        }

        return loot.ToArray();
    }

    private string RandomItem() => GameManager.instance.referenceItems[Random.Range(0, GameManager.instance.referenceItems.Length)].name;

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1);
        EnemyAttack();
        yield return new WaitForSeconds(1);
        NextTurn();
    }

    public void EnemyAttack()
    {
        var playerIndices = activeBattlers.WithIndex().AllPlayers().NotDead().Select(x => x.index).ToArray();
        int indexToAttack = playerIndices[Random.Range(0, playerIndices.Length)];

        int moveIndex = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);

        var move = movesList.FirstOrDefault(x => x.moveName == activeBattlers[currentTurn].movesAvailable[moveIndex]);
        if (move == null)
        {
            Debug.LogError($"Unknown move encountered '{activeBattlers[currentTurn].movesAvailable[moveIndex]}'");
            return;
        }

        Instantiate(move.theEffect, activeBattlers[indexToAttack].transform.position, activeBattlers[indexToAttack].transform.rotation);

        Instantiate(enemyAttackEffect, ActiveBattler.transform.position, ActiveBattler.transform.rotation);

        DealDamage(indexToAttack, move);


    }

    public void DealDamage(int target, BattleMove move)
    {
        int dmg = Convert.ToInt32((ActiveBattler.TotalAttackPower / Math.Max(1, activeBattlers[target].TotalDefensePower)) * move.movePower * Random.Range(0.9f, 1.1f));
        activeBattlers[target].currentHP = Math.Max(activeBattlers[target].currentHP - dmg, 0);
        Instantiate(damageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(dmg);
        Debug.Log($"Attacker ({ActiveBattler.charName}) hit {activeBattlers[target].charName} with {move.moveName} for {dmg} damage.");
        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        int maxIndex = 0;

        foreach ((var player, int index) in activeBattlers.WithIndex().AllPlayers())
        {
            playerNames[index].gameObject.SetActive(true);
            playerNames[index].text = player.charName;
            playerHP[index].text = $"{player.currentHP} / {player.maxHP}";
            playerMP[index].text = $"{player.currentMP} / {player.maxMP}";
            maxIndex = index;
        }

        while (++maxIndex < playerNames.Length)
        {
            playerNames[maxIndex].gameObject.SetActive(false);
        }
    }

    public void PlayerAttack(int target, string moveName)
    {
        var move = movesList.FirstOrDefault(x => x.moveName == moveName);
        if (move == null)
        {
            Debug.LogError($"Unknown move encountered '{moveName}'");
            return;
        }

        Instantiate(move.theEffect, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation);
        Instantiate(enemyAttackEffect, ActiveBattler.transform.position, ActiveBattler.transform.rotation);

        DealDamage(target, move);

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);

        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);
        magicMenu.SetActive(false);

        int currentButton = 0;
        foreach (var enemy in activeBattlers.WithIndex().AllEnemies().NotDead())
        {
            targetButtons[currentButton].gameObject.SetActive(true);
            targetButtons[currentButton].targetName.text = enemy.item.charName;
            targetButtons[currentButton].activeBattlerTarget = enemy.index;
            targetButtons[currentButton].moveName = moveName;
            currentButton++;
        }

        while (currentButton < targetButtons.Length)
        {
            targetButtons[currentButton++].gameObject.SetActive(false);
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);
        targetMenu.SetActive(false);

        foreach (var button in magicButtons.WithIndex())
        {
            if ((ActiveBattler?.movesAvailable.Length ?? 0) > button.index)
            {
                string moveName = ActiveBattler.movesAvailable[button.index];
                button.item.gameObject.SetActive(true);
                button.item.spellName = moveName;
                button.item.spellCost = GetMove(moveName).moveCost;
                button.item.costText.color = (button.item.spellCost > ActiveBattler.currentMP) ? Color.red : Global.Colors.Default;
            }
            else
            {
                button.item.gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if (Random.Range(0, 100) < chanceToFlee)
        {
            battleActive = false;
            StartCoroutine(ExitBattleCo(CombatEndCondition.PlayerFlee));
        }
        else
        {
            NextTurn();
            notification.Activate("Escape failed!");
        }
    }
}
