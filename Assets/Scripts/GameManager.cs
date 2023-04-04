using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharStats[] playerStats;

    public bool gameMenuOpen;
    public bool dialogActive;
    public bool fadingBetweenAreas;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItems;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController.instance.canMove = !(gameMenuOpen || dialogActive || fadingBetweenAreas);

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem("Iron Armor", 3);
            RemoveItem("Health Potion", 1);
        }
    }

    public bool CanOpenMenu() => !(dialogActive || fadingBetweenAreas);

    public Item GetItemDetails(string itemToGrab)
    {
        for (int x = 0; x < referenceItems.Length; x++)
        {
            if (referenceItems[x].itemName == itemToGrab)
            {
                return referenceItems[x];
            }
        }

        return null;
    }

    public void SortItems()
    {
        for (int x = 0; x < itemsHeld.Length; x++)
        {
            if ((x > 0) && (itemsHeld[x] != ""))
            {
                int y = x - 1;

                while ((y >= 0) && (itemsHeld[y] == ""))
                {
                    itemsHeld[y] = itemsHeld[y + 1];
                    itemsHeld[y + 1] = "";

                    numberOfItems[y] = numberOfItems[y + 1];
                    numberOfItems[y + 1] = 0;
                    y--;
                }
            }
        }
    }

    public void AddItem(string itemToAdd, int quantity)
    {
        SortItems();

        bool itemValid = false;

        // Validate itemToAdd
        for (int x = 0; x < referenceItems.Length; x++)
        {
            if (referenceItems[x].itemName == itemToAdd)
            {
                itemValid = true;
                break;
            }
        }

        if (!itemValid)
        {
            Debug.Log($"Invalid item encountered: '{itemToAdd}'");
            return; // should error...
        }

        for (int x = 0; x < itemsHeld.Length; x++)
        {
            if ((itemsHeld[x] == itemToAdd) || (itemsHeld[x] == ""))
            {
                itemsHeld[x] = itemToAdd;
                numberOfItems[x] += quantity;
                break;
            }
        }

        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove, int quantity)
    {
        for (int x = 0; x < itemsHeld.Length; x++)
        {
            if (itemsHeld[x] == itemToRemove)
            {
                numberOfItems[x] = System.Math.Max(0, numberOfItems[x] - quantity);
                if (numberOfItems[x] == 0)
                {
                    itemsHeld[x] = "";
                }

                GameMenu.instance.ShowItems();
                return;
            }
        }

        Debug.Log($"Unable to remove item '{itemToRemove}', not found in inventory");
    }
}
