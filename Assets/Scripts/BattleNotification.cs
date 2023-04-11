using UnityEngine;
using UnityEngine.UI;

public class BattleNotification : MonoBehaviour
{
    public float awakeTime;
    private float awakeCounter;
    public Text theText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        awakeCounter -= Time.deltaTime;

        if (awakeCounter <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Activate(string text, float duration = 1.0f)
    {
        theText.text = text;
        gameObject.SetActive(true);
        awakeCounter = awakeTime = duration;
    }
}
