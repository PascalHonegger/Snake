using UnityEngine;

public class BodyPartController : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;

	public Sprite UpSprite;
	public Sprite UpLeftSprite;
	public Sprite LeftSprite;
	public Sprite LeftDownSprite;
	public Sprite DownSprite;
	public Sprite DownRightSprite;
	public Sprite RightSprite;
	public Sprite RightUpSprite;

	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
}
