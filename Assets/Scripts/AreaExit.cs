using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
	public string areaToLoad;

	public string areaTransitionName;

	public AreaEntrance theEntrance;

	public float waitToLoad = 1f;

	private bool shouldLoadAfterFade;

    // Start is called before the first frame update
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
    }

    // Update is called once per frame
    void Update()
    {
		if (shouldLoadAfterFade)
		{
			waitToLoad -= Time.deltaTime;	// we should actually tie this to how long the other thing takes, or at least use the save variable
			if (waitToLoad <= 0)
			{
				shouldLoadAfterFade = false;
				SceneManager.LoadScene(areaToLoad);
			}
		}
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			GameManager.instance.fadingBetweenAreas = true;

			shouldLoadAfterFade = true;
			UIFade.instance.FadeToBlack();

			PlayerController.instance.areaTransitionName = areaTransitionName;
		}
	}
}
