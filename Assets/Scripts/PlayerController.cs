using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;
	
	public Animator myAnim; 
	
	public static PlayerController instance;
	
	public string areaTransitionName;

    // Start is called before the first frame update
    void Start()
    {
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		
		DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
		float rawX = Input.GetAxisRaw("Horizontal");
		float rawY = Input.GetAxisRaw("Vertical");
		
        theRB.velocity = new Vector2(rawX, rawY) * moveSpeed;
		
		myAnim.SetFloat("moveX", theRB.velocity.x);
		myAnim.SetFloat("moveY", theRB.velocity.y);

		if ((rawX != 0) || (rawY != 0))
		{
			myAnim.SetFloat("lastMoveX", rawX);
			myAnim.SetFloat("lastMoveY", rawY);
		}
    }
}
