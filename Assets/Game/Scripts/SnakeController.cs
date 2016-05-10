using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class SnakeController : MonoBehaviour
{
	public float MoveTime;

	private Rigidbody2D rb;

	private Vector3 _turn = new Vector3
	{
		x = 0,
		y = 0
	};

	// Use this for initialization
	void Start () {
		InvokeRepeating("Move", MoveTime, MoveTime);

		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if (CrossPlatformInputManager.GetAxis("Horizontal") < 0)
		{
			_turn.z = 90;
		}
		else if (CrossPlatformInputManager.GetAxis("Vertical") > 0)
		{
			_turn.z = 0;
		}
		else if (CrossPlatformInputManager.GetAxis("Horizontal") > 0)
		{
			_turn.z = 270;
		}
		else if (CrossPlatformInputManager.GetAxis("Vertical") < 0)
		{
			_turn.z = 180;
		}

		transform.eulerAngles = _turn;
	}

	private void Move()
	{
		transform.Translate(Vector3.up);
	}
}
