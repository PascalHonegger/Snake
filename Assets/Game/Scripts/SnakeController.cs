using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class SnakeController : MonoBehaviour
{
	public float MoveTime;

	public Vector3 StartPosition;

	public GameObject BodyPrefab;

	[Header("Score")]
	public Text ScoreText;
	public Text HighscoreText;

	public int Highscore;

	private Vector3 _newPosition;

	private int Score
	{
		get { return _score; }
		set
		{
			_score = value;
			if (Highscore < _score)
			{
				Highscore = _score;
			}

			ScoreText.text = "Score: " + _score;
			HighscoreText.text = "Highscore: " + Highscore;
		}
	}

	private MoveAndGrowController MoveScript
	{
		get { return GetComponent<MoveAndGrowController>(); }
	}

	void Start()
	{
		Score = 0;

		MoveScript.DestroyOther();

		transform.localPosition = StartPosition;

		_lastMovement = _currentDirection = MoveDirection.Up;

		_gameOver = false;

		MoveScript.CreateDefaultSnake(BodyPrefab);

		Time.timeScale = 1;

		InvokeRepeating("Move", 0, MoveTime);
	}

	private MoveDirection _currentDirection;
	private MoveDirection _lastMovement;

	private void CalculateDirection()
	{
		var horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
		var verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");

		if (Mathf.Abs(horizontalAxis) > Mathf.Abs(verticalAxis))
		{
			if (horizontalAxis < 0 && _lastMovement != MoveDirection.Right)
			{
				_currentDirection = MoveDirection.Left;
			}
			else if (_lastMovement != MoveDirection.Left)
			{
				_currentDirection = MoveDirection.Right;
			}
		}
		else if (Mathf.Abs(horizontalAxis) < Mathf.Abs(verticalAxis))
		{
			if (verticalAxis > 0 && _lastMovement != MoveDirection.Down)
			{
				_currentDirection = MoveDirection.Up;
			}
			else if (_lastMovement != MoveDirection.Up)
			{
				_currentDirection = MoveDirection.Down;
			}
		}
	}

	private void CalculateNewPosition()
	{
		CalculateDirection();

		var x = transform.localPosition.x;
		var y = transform.localPosition.y;

		switch (_currentDirection)
		{
			case MoveDirection.Left:
				x--;
				break;
			case MoveDirection.Up:
				y++;
				break;
			case MoveDirection.Right:
				x++;
				break;
			case MoveDirection.Down:
				y--;
				break;
			case MoveDirection.None:
				throw new ArgumentOutOfRangeException();
			default:
				throw new ArgumentOutOfRangeException();
		}

		_newPosition = new Vector3(x, y);
	}

	private bool _gameOver;
	private int _score;
	private int _highscore;

	void OnGUI()
	{
		if (_gameOver)
		{
			if (GUI.Button(new Rect(Screen.width * .3f, Screen.height * .3f, Screen.width * .4f, Screen.height * .4f), "Restart?"))
			{
				Start();
			}
		}
	}

	public void Update()
	{
		CalculateDirection();
	}

	private void Move()
	{
		RaycastHit2D hit;

		CalculateNewPosition();

		//Check for collision
		if (TryMove(_newPosition, out hit))
		{
			//No collision
			_lastMovement = _currentDirection;
			MoveScript.MoveTo(_newPosition);
			MoveScript.GetComponent<BodyPartController>().AdjustTexture();
		}
		else
		{
			//Collision detected
			var other = hit.collider.gameObject;
			if (other.CompareTag("Wall") || other.CompareTag("Body"))
			{
				CancelInvoke("Move");
				_gameOver = true;
				Time.timeScale = 0;
			}
			else if (other.gameObject.CompareTag("Fruit"))
			{
				Score++;

				MoveScript.AddBodyPart(BodyPrefab, null);

				MoveScript.MoveTo(_newPosition);
				MoveScript.GetComponent<BodyPartController>().AdjustTexture();
			}
		}
	}

	private bool TryMove(Vector3 endpoint, out RaycastHit2D hit)
	{
		GetComponent<BoxCollider2D>().enabled = false;

		hit = Physics2D.Linecast(transform.localPosition, endpoint);

		GetComponent<BoxCollider2D>().enabled = true;

		return hit.transform == null;
	}
}
