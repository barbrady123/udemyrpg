using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;

    public GameObject uiButtonsHolder;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

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
            uiButtonsHolder.SetActive(activeBattlers[currentTurn].IsPlayer);

            if (!activeBattlers[currentTurn].IsPlayer)
            {
                // enemy attack...
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (battleActive)
            return;

        battleActive = true;
        GameManager.instance.battleActive = true;

        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        battleScene.SetActive(true);

        AudioManager.instance.PlayBGM(6);

        turnWaiting = true;
        currentTurn = Random.Range(0, activeBattlers.Count);

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

            newPlayer.charName = stats.charName;
            newPlayer.currentHP = stats.currentHP;
            newPlayer.maxHP = stats.maxHP;
            newPlayer.currentMP = stats.currentMP;
            newPlayer.maxMP = stats.maxMP;
            newPlayer.strength = stats.strength;
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

            newEnemy.charName = prefab.charName;
            newEnemy.currentHP = prefab.currentHP;
            newEnemy.maxHP = prefab.maxHP;
            newEnemy.currentMP = prefab.currentMP;
            newEnemy.maxMP = prefab.maxMP;
            newEnemy.strength = prefab.strength;
            newEnemy.weaponPower = prefab.weaponPower;
            newEnemy.armorPower = prefab.armorPower;
            newEnemy.hasDied = prefab.currentHP <= 0;

            activeBattlers.Add(newEnemy);
        }
    }
}
