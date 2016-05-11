using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SnakeController : MonoBehaviour
{
	public float MoveTime;

	public GameObject BodyPrefab;

	private Vector3 _turn = new Vector3
	{
		x = 0,
		y = 0
	};

	// Use this for initialization
	void Start () {
		InvokeRepeating("Move", MoveTime, MoveTime);
	}

	// Update is called once per frame
	void Update()
	{
		var horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
		var verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");

		if (Mathf.Abs(horizontalAxis) > Mathf.Abs(verticalAxis))
		{
			if (horizontalAxis < 0)
			{
				_turn.z = 90;
			}
			else
			{
				_turn.z = 270;
			}
		}
		else if (Mathf.Abs(horizontalAxis) < Mathf.Abs(verticalAxis))
		{
			if (verticalAxis < 0)
			{
				_turn.z = 180;
			}
			else
			{
				_turn.z = 0;
			}
		}

		transform.eulerAngles = _turn;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Body")
		{
			Time.timeScale = 0;
		}
		else if (collision.gameObject.tag == "Fruit")
		{
			collision.gameObject.GetComponent<FruitScript>().MoveAway();
			Instantiate(BodyPrefab, new Vector3(transform.localPosition.x, transform.localPosition.y - 1), transform.localRotation);
		}
	}

	private void Move()
	{
		transform.Translate(Vector3.up);
	}
}
