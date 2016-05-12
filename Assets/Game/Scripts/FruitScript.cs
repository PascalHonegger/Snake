using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
	public Sprite[] Sprites;

	private SpriteRenderer _spriteRenderer;
	private Queue<Sprite> _spriteQueue;

	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		MoveAway();
	}

	private void FillQueueIfEmtpy()
	{
		if (_spriteQueue == null || !_spriteQueue.Any())
		{
			_spriteQueue = new Queue<Sprite>(Sprites);
		}
	}

	public void MoveAway()
	{
		FillQueueIfEmtpy();
		_spriteRenderer.sprite = _spriteQueue.Dequeue();

		transform.Translate(Vector3.left);
	}
}