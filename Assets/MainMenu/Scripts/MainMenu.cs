using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public Texture BackgroundTexture;

	public float RelativeDistanceLeft;
	public float RelativeButtonWidth;
	public float RelativeButtonHeight;

	private float DistanceLeft
	{
		get { return Screen.width * RelativeDistanceLeft; }
	}

	private float ButtonWidth
	{
		get { return Screen.width * RelativeButtonWidth; }
	}

	private float ButtonHeight
	{
		get { return Screen.height* RelativeButtonHeight; }
	}

	private string _text;

	void OnGUI() {
		// Display Background Texture
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BackgroundTexture);

		//Display Buttons
		if(GUI.Button(new Rect(DistanceLeft, Screen.height*.3f, ButtonWidth, ButtonHeight), "Play"))
		{
			_text = "Start pressed!";
		}
		if (GUI.Button(new Rect(DistanceLeft, Screen.height * .5f, ButtonWidth, ButtonHeight), "Options"))
		{
			_text = "Options pressed!";
		}
		if (GUI.Button(new Rect(DistanceLeft, Screen.height * .7f, ButtonWidth, ButtonHeight), "Highscore"))
		{
			_text = "Highscore pressed!";
		}

		if (_text != null)
		{
			_text = GUI.TextField(new Rect(DistanceLeft, Screen.height * .1f, ButtonWidth, ButtonHeight), _text);
		}
	}
}
