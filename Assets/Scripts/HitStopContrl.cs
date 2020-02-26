using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopContrl : MonoBehaviour 
{
	bool waiting;

	public void Stop(float duration)
	{
		if(waiting)
			return;

		Time.timeScale = 0.5f;
		StartCoroutine(Wait(duration));
	}


	IEnumerator Wait(float duration)
	{
		waiting	= false;
		yield return new WaitForSecondsRealtime(duration);
		Time.timeScale = 1.0f;
		waiting = false;
	}
}
