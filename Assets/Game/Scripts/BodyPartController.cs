using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyPartController : MonoBehaviour
{
	public Sprite HeadLeftSprite;
	public Sprite HeadUpSprite;
	public Sprite HeadRightSprite;
	public Sprite HeadDownSprite;

	public Sprite TailLeftSprite;
	public Sprite TailUpSprite;
	public Sprite TailRightSprite;
	public Sprite TailDownSprite;

	public Sprite UpDownSprite;
	public Sprite LeftRightSprite;

	public Sprite UpLeftSprite;
	public Sprite UpRightSprite;

	public Sprite DownLeftSprite;
	public Sprite DownRightSprite;

	public void SetTexture(MoveDirection previousPartDirection, MoveDirection nextPartDirection)
	{
		Sprite sprite = null;

		if (nextPartDirection == MoveDirection.None)
		{
			switch (previousPartDirection)
			{
				case MoveDirection.Left:
					sprite = TailRightSprite;
					break;
				case MoveDirection.Up:
					sprite = TailDownSprite;
					break;
				case MoveDirection.Right:
					sprite = TailLeftSprite;
					break;
				case MoveDirection.Down:
					sprite = TailUpSprite;
					break;
				case MoveDirection.None:
					throw new ArgumentOutOfRangeException("previousPartDirection", previousPartDirection, null);
				default:
					throw new ArgumentOutOfRangeException("previousPartDirection", previousPartDirection, null);
			}
		}
		else if (previousPartDirection == MoveDirection.None)
		{
			switch (nextPartDirection)
			{
				case MoveDirection.Left:
					sprite = HeadRightSprite;
					break;
				case MoveDirection.Up:
					sprite = HeadDownSprite;
					break;
				case MoveDirection.Right:
					sprite = HeadLeftSprite;
					break;
				case MoveDirection.Down:
					sprite = HeadUpSprite;
					break;
				case MoveDirection.None:
					throw new ArgumentOutOfRangeException("previousPartDirection", previousPartDirection, null);
				default:
					throw new ArgumentOutOfRangeException("previousPartDirection", previousPartDirection, null);
			}
		}
		else
		{
			if ((previousPartDirection == MoveDirection.Up && nextPartDirection == MoveDirection.Left)
				|| (previousPartDirection == MoveDirection.Left && nextPartDirection == MoveDirection.Up))
			{
				sprite = UpLeftSprite;
			}
			else if ((previousPartDirection == MoveDirection.Up && nextPartDirection == MoveDirection.Right)
				|| (previousPartDirection == MoveDirection.Right && nextPartDirection == MoveDirection.Up))
			{
				sprite = UpRightSprite;
			}
			else if ((previousPartDirection == MoveDirection.Up && nextPartDirection == MoveDirection.Down)
				|| (previousPartDirection == MoveDirection.Down && nextPartDirection == MoveDirection.Up))
			{
				sprite = UpDownSprite;
			}
			else if ((previousPartDirection == MoveDirection.Left && nextPartDirection == MoveDirection.Right)
				|| (previousPartDirection == MoveDirection.Right && nextPartDirection == MoveDirection.Left))
			{
				sprite = LeftRightSprite;
			}
			else if ((previousPartDirection == MoveDirection.Down && nextPartDirection == MoveDirection.Right)
				|| (previousPartDirection == MoveDirection.Right && nextPartDirection == MoveDirection.Down))
			{
				sprite = DownRightSprite;
			}
			else if ((previousPartDirection == MoveDirection.Down && nextPartDirection == MoveDirection.Left)
				|| (previousPartDirection == MoveDirection.Left && nextPartDirection == MoveDirection.Down))
			{
				sprite = DownLeftSprite;
			}
		}

		gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	}

	private MoveAndGrowController MoveScript
	{
		get { return gameObject.GetComponent<MoveAndGrowController>(); }
	}

	public void AdjustTexture()
	{
		SetTexture(MoveScript.PreviousBodyPart == null ? MoveDirection.None : RelativePosition(MoveScript.PreviousBodyPart.transform.localPosition), MoveScript.NextBodyPart == null ? MoveDirection.None : RelativePosition(MoveScript.NextBodyPart.transform.localPosition));
		if (MoveScript.NextBodyPart != null) MoveScript.NextBodyPart.GetComponent<BodyPartController>().AdjustTexture();
	}

	private MoveDirection RelativePosition(Vector3 otherPosition)
	{
		if (Mathf.Floor(transform.localPosition.x) > Mathf.Floor(otherPosition.x))
		{
			return MoveDirection.Left;
		}

		if (Mathf.Floor(transform.localPosition.x) < Mathf.Floor(otherPosition.x))
		{
			return MoveDirection.Right;
		}

		return Mathf.Floor(transform.localPosition.y) < Mathf.Floor(otherPosition.y) ? MoveDirection.Up : MoveDirection.Down;
	}
}
