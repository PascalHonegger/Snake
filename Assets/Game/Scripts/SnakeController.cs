using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SnakeController : MonoBehaviour
{
	public float MoveTime;

	public Vector3 StartPosition;

	public GameObject BodyPrefab;

	private Vector3 _turn = new Vector3
	{
		x = 0,
		y = 0
	};

	private MoveAndGrowController MoveScript
	{
		get { return GetComponent<MoveAndGrowController>(); }
	}

	void Start ()
	{
		MoveScript.DestroyOther();

		transform.localPosition = StartPosition;

		_turn.z = 0;
		Update();

		_gameOver = false;

		MoveScript.CreateDefaultSnake(BodyPrefab);

		Time.timeScale = 1;

		InvokeRepeating("Move", MoveTime, MoveTime);
	}

	void Update()
	{
		var horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
		var verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");

		if (Mathf.Abs(horizontalAxis) > Mathf.Abs(verticalAxis))
		{
			if (horizontalAxis < 0 && Mathf.RoundToInt(MoveScript.NextBodyPart.transform.localPosition.x) >= Mathf.RoundToInt(transform.localPosition.x))
			{
				_turn.z = 90;
			}
			else if(Mathf.RoundToInt(MoveScript.NextBodyPart.transform.localPosition.x) <= Mathf.RoundToInt(transform.localPosition.x))
			{
				_turn.z = 270;
			}
		}
		else if (Mathf.Abs(horizontalAxis) < Mathf.Abs(verticalAxis))
		{
			if (verticalAxis < 0 && Mathf.RoundToInt(MoveScript.NextBodyPart.transform.localPosition.y) >= Mathf.RoundToInt(transform.localPosition.y))
			{
				_turn.z = 180;
			}
			else if (Mathf.RoundToInt(MoveScript.NextBodyPart.transform.localPosition.y) <= Mathf.RoundToInt(transform.localPosition.y))
			{
				_turn.z = 0;
			}
		}

		transform.eulerAngles = _turn;
	}

	private bool _gameOver;

	void OnGUI()
	{
		if (_gameOver)
		{
			if (GUI.Button(new Rect(Screen.width*.3f, Screen.height*.3f, Screen.width*.4f, Screen.height*.4f), "Restart?"))
			{
				Start();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Body")
		{
			Time.timeScale = 0;
			CancelInvoke("Move");
			_gameOver = true;
		}
		else if (collision.gameObject.tag == "Fruit")
		{
			collision.gameObject.GetComponent<FruitScript>().MoveAway();
			MoveScript.AddBodyPart(BodyPrefab, MoveScript);
		}
	}

	private void Move()
	{
		MoveScript.MoveTo(transform.localPosition);
		transform.Translate(Vector3.up);
		MoveScript.NextBodyPart.GetComponent<BodyPartController>().AdjustTexture();
	}
}
