using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SnakeController : MonoBehaviour
{
	public float MoveTime;

	public Vector3 StartPosition;

	public GameObject BodyPrefab;

	public BoxCollider2D boxCollider;

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

		Time.timeScale = 1;

		MoveScript.CreateDefaultSnake(BodyPrefab);

		InvokeRepeating("Move", MoveTime, MoveTime);
	}

	void Update()
	{
		var horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
		var verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");

		if (Mathf.Abs(horizontalAxis) > Mathf.Abs(verticalAxis))
		{
			if (horizontalAxis < 0 && Mathf.Floor(MoveScript.NextBodyPart.transform.localPosition.x) >= Mathf.Floor(transform.localPosition.x))
			{
				_turn.z = 90;
			}
			else if(Mathf.Floor(MoveScript.NextBodyPart.transform.localPosition.x) <= Mathf.Floor(transform.localPosition.x))
			{
				_turn.z = 270;
			}
		}
		else if (Mathf.Abs(horizontalAxis) < Mathf.Abs(verticalAxis))
		{
			if (verticalAxis < 0 && Mathf.Floor(MoveScript.NextBodyPart.transform.localPosition.y) >= Mathf.Floor(transform.localPosition.y))
			{
				_turn.z = 180;
			}
			else if (Mathf.Floor(MoveScript.NextBodyPart.transform.localPosition.y) <= Mathf.Floor(transform.localPosition.y))
			{
				_turn.z = 0;
			}
		}

		transform.eulerAngles = _turn;
	}

	private bool _gameOver = false;

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
			GetComponent<MoveAndGrowController>().AddBodyPart(BodyPrefab, MoveScript);
		}
	}
	
	private void Move()
	{
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;
		switch (Mathf.RoundToInt(_turn.z))
		{
			case 0:
				y++;
				break;
			case 90:
				x--;
				break;
			case 180:
				y--;
				break;
			case 270:
				x++;
				break;
		}


		Vector3 endpoint = new Vector3(x,y);

		if (!gameObject.CompareTag("Player") || (TryMove(endpoint) && gameObject.CompareTag("Player")))
		{
			MoveScript.MoveTo(transform.localPosition);
			transform.Translate(Vector3.up);
			MoveScript.NextBodyPart.GetComponent<BodyPartController>().AdjustTexture();
		}
		else
		{
			Time.timeScale = 0;
			CancelInvoke("Move");
			_gameOver = true;
		}
	}

	public bool TryMove(Vector3 endpoint)
	{
		boxCollider.enabled = false;
		RaycastHit2D hit;

		hit = Physics2D.Linecast(transform.localPosition, endpoint);

		boxCollider.enabled = true;

		if (hit.transform == null || hit.collider.gameObject.CompareTag("Fruit"))
		{
			return true;
		}

		return false;

	}
}
