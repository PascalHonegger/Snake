using System;
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
		var horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
		var verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");

		if (Mathf.Abs(horizontalAxis) > Mathf.Abs(verticalAxis))
		{
			_turn.z = horizontalAxis < 0 ? 90 : 270;
		}
		else if (Mathf.Abs(horizontalAxis) < Mathf.Abs(verticalAxis))
		{
			_turn.z = verticalAxis < 0 ? 180 : 0;
		}

		transform.eulerAngles = _turn;
	}

	void OnCollisonEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
			Time.timeScale = 0;
		}
	}

	private void Move()
	{
		transform.Translate(Vector3.up);
	}
}
