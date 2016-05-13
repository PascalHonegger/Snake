using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public Texture BackgroundTexture;
	public Texture VolumeTexture;

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

	void Start()
	{
		AudioListener.volume = 0.5f;
	}

	void OnGUI()
	{
		// Display Background Texture
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), BackgroundTexture);

		//Display Buttons
		if(GUI.Button(new Rect(DistanceLeft, Screen.height*.3f, ButtonWidth, ButtonHeight), "Play"))
		{
			SceneManager.LoadScene(1);
		}

		GUI.Label(new Rect(DistanceLeft, Screen.height * .7f - 50, ButtonWidth, 100), VolumeTexture);

		AudioListener.volume = GUI.HorizontalSlider(new Rect(DistanceLeft + 100, Screen.height * .7f, ButtonWidth - 100, 20), AudioListener.volume, 0, 1);
	}
}
