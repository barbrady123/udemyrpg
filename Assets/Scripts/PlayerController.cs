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

	private Vector3 bottomLeftLimit;
	private Vector3 topRightLimit;

	public bool canMove = true;

	private static readonly int idMoveX = Animator.StringToHash("moveX");
	private static readonly int idMoveY = Animator.StringToHash("moveY");
	private static readonly int idLastMoveX = Animator.StringToHash("lastMoveX");
	private static readonly int idLastMoveY = Animator.StringToHash("lastMoveY");

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
		if (GameManager.instance.loadingScene)
		{
			transform.position = GameManager.instance.saveData.Position;
			GameManager.instance.loadingScene = false;
		}

		float rawX = Input.GetAxisRaw(Global.Inputs.AxisHorizontal);
		float rawY = Input.GetAxisRaw(Global.Inputs.AxisVertical);

		if (canMove)
		{
			var newVelocity = 
				new Vector2(rawX, rawY) 
				* moveSpeed 
				* (Input.GetKey(KeyCode.LeftShift) ? Global.Physics.RunSpeedModifier : 1.0f);

	        theRB.velocity = newVelocity;

			if ((rawX != 0) || (rawY != 0))
			{
				myAnim.SetFloat(idLastMoveX, rawX);
				myAnim.SetFloat(idLastMoveY, rawY);
			}
		}
		else
		{
			theRB.velocity = Vector2.zero;
		}

		myAnim.SetFloat(idMoveX, theRB.velocity.x);
		myAnim.SetFloat(idMoveY, theRB.velocity.y);

        // keep the player inside the bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
            transform.position.z);
    }

	public void SetBounds(Vector3 botLeft, Vector3 topRight)
	{
		bottomLeftLimit = botLeft + new Vector3(0.5f, 1, 0);
		topRightLimit = topRight + new Vector3(-0.5f, -1, 0);
	}
}
