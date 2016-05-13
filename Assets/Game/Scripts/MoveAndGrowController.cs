using UnityEngine;

public class MoveAndGrowController : MonoBehaviour {

	public MoveAndGrowController PreviousBodyPart;
	public MoveAndGrowController NextBodyPart;

	public void AddBodyPart(GameObject bodyPrefab, MoveAndGrowController previousBodyPart)
	{
		PreviousBodyPart = previousBodyPart;

		if (NextBodyPart != null)
		{
			NextBodyPart.AddBodyPart(bodyPrefab, this);
		}
		else
		{
			CreateBodyPart(bodyPrefab, transform.localPosition);
		}
	}

	public void DestroyOther()
	{
		if (NextBodyPart != null)
		{
			NextBodyPart.GetComponent<MoveAndGrowController>().Destroy();
		}
	}

	private void Destroy()
	{
		DestroyOther();
		Destroy(gameObject);
	}

	private void CreateBodyPart(GameObject bodyPrefab, Vector3 location)
	{
		var part = Instantiate(bodyPrefab);
		var script = part.GetComponent<MoveAndGrowController>();
		script.PreviousBodyPart = this;
		script.MoveTo(location);

		NextBodyPart = script;
	}

	public void MoveTo(Vector3 localPosition)
	{

			if (NextBodyPart != null)
			{
				NextBodyPart.GetComponent<MoveAndGrowController>().MoveTo(transform.localPosition);
			}

			transform.localPosition = localPosition;

	}

	public void CreateDefaultSnake(GameObject bodyPrefab)
	{
		CreateBodyPart(bodyPrefab, new Vector3(transform.localPosition.x, transform.localPosition.y - 1));
		AddBodyPart(bodyPrefab, this);
	}


}
