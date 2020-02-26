using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusOn : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		Invoke("CameraNormal",0.5f);
			
	}

	public void CameraFocus()
	{
		Camera.main.orthographicSize = 4;
		//transform.position = new Vector3 (FindObjectOfType<Player>().transform.position.x,transform.position.y,transform.position.z);
	}

	private void CameraNormal()
	{
		Camera.main.orthographicSize = Mathf.Lerp (Camera.main.orthographicSize,5,0.04f);
	}
}
