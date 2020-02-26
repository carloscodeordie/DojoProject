using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSet : MonoBehaviour 
{
	public bool fullScreen = true; 
	void Awake()
	{
		Application.targetFrameRate = 45;
	}

	void Start () {
		Screen.SetResolution (1280,720,fullScreen);
		Cursor.visible = false;
	}
	

	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftShift)) 
		{
			Screen.SetResolution (1280, 720, true);
			Cursor.visible = false;
		} 
		else if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Screen.SetResolution (1280, 720, false);
			Cursor.visible = true;
		}
	}

	void OnGUI()
	{
		string text = string.Format ("FPS:{0}", 1.0f / Time.smoothDeltaTime);
		GUIStyle font = new GUIStyle ();
		font.normal.textColor = Color.white;
		font.fontSize = 20;

		GUI.Label (new Rect(Screen.width-200,Screen.height-700,200,200),text,font);
	}
}
