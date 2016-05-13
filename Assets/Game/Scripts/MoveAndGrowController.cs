using UnityEngine;

public class MoveAndGrowController : MonoBehaviour {

	public GameObject NextBodyPart;

	public void AddBodyPart(GameObject bodyPrefab)
	{
		if (NextBodyPart != null)
		{
			NextBodyPart.GetComponent<MoveAndGrowController>().AddBodyPart(bodyPrefab);
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

	public void Destroy()
	{
		DestroyOther();
		Destroy(gameObject);
	}

	private void CreateBodyPart(GameObject bodyPrefab, Vector3 location)
	{
		var part = Instantiate(bodyPrefab);
		part.GetComponent<MoveAndGrowController>().MoveTo(location);
		NextBodyPart = part;
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
		AddBodyPart(bodyPrefab);
	}


}
