using UnityEngine;
using System.Collections;

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
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
		{
			_turn.z = 90;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			_turn.z = 0;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
		{
			_turn.z = 270;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
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
