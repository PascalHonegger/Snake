using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class SnakeController : MonoBehaviour
{
	public float MoveTime;

	public Vector3 StartPosition;

	public GameObject BodyPrefab;

	public BoxCollider2D boxCollider;

	private Vector3 _newPosition;

	private MoveAndGrowController MoveScript
	{
		get { return GetComponent<MoveAndGrowController>(); }
	}

	void Start ()
	{
		MoveScript.DestroyOther();

		transform.localPosition = StartPosition;

		_currentDirection = MoveDirection.Up;

		_gameOver = false;

		MoveScript.CreateDefaultSnake(BodyPrefab);

		Time.timeScale = 1;

		InvokeRepeating("Move", 0, MoveTime);
	}

	private MoveDirection _currentDirection;

	private void CalculateNewPosition()
	{
		var horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
		var verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");

		if (Mathf.Abs(horizontalAxis) > Mathf.Abs(verticalAxis))
		{
			if (horizontalAxis < 0 && _currentDirection != MoveDirection.Right)
			{
				_currentDirection = MoveDirection.Left;
			}
			else if(_currentDirection != MoveDirection.Left)
			{
				_currentDirection = MoveDirection.Right;
			}
		}
		else if (Mathf.Abs(horizontalAxis) < Mathf.Abs(verticalAxis))
		{
			if (verticalAxis > 0 && _currentDirection != MoveDirection.Down)
			{
				_currentDirection = MoveDirection.Up;
			}
			else if (_currentDirection != MoveDirection.Up)
			{
				_currentDirection = MoveDirection.Down;
			}
		}

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

	private void Move()
	{
		CalculateNewPosition();

		RaycastHit2D hit;

		//Check for collision
		if (TryMove(_newPosition, out hit))
		{
			//No collision
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
				other.GetComponent<FruitScript>().MoveAway();
				MoveScript.AddBodyPart(BodyPrefab, null);
			}
		}
	}

	private bool TryMove(Vector3 endpoint, out RaycastHit2D hit)
	{
		boxCollider.enabled = false;

		hit = Physics2D.Linecast(transform.localPosition, endpoint);

		boxCollider.enabled = true;

		return hit.transform == null;
	}
}
