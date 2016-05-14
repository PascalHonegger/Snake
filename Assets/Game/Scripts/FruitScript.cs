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

		ChangePosition();
	}

	private void ChangePosition()
	{
		transform.localPosition = new Vector3(Random.Range(-15, 14) + 0.5f, Random.Range(-8, 7) + 0.5f);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		ChangePosition();
	}
}