using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MoonWalker : Enemy
{
	public static MoonWalker instance;
	public GameObject hat;
	GameObject tempHat;
	public Transform hatPos;
	public float minBoomerangTime,maxBoomerangTime;


	public GameObject ghostShadow;
	

	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
		Invoke ("SpecialAttack",Random.Range (minBoomerangTime,maxBoomerangTime));
		instance = this;
	}

	float timer = 0.2f;
	public override void Update()
	{
		base.Update ();
		if (currentSpeed > 1) 
		{
			timer -= Time.deltaTime;
			
			if(timer <= 0)
			{
				timer = 0.02f;
				GameObject shadow = Instantiate (ghostShadow, transform.position, transform.rotation) as GameObject;
			}
		}
	}

	void SpecialAttack()
	{
		if(!isDead&&!highDamage&&!damaged)
		{
			Debug.Log ("Fuck you");
			anim.SetTrigger ("Boomerang");
		}
		Invoke ("SpecialAttack",Random.Range(minBoomerangTime,maxBoomerangTime));
	}

	public void HatAttack()
	{
		tempHat = Instantiate (hat,hatPos.transform.position,hatPos.transform.rotation);
		if(facingRight)
		{
			tempHat.GetComponent<Boomerang>().direction = 1;
		}
		else
		{
			tempHat.GetComponent<Boomerang>().direction = -1;
		}
	}

	public void MaxSpeed()
	{
		currentSpeed = 2 * maxSpeed;
	}

	private void BossDefeated()
	{
		MusicController.instance.PlaySong (MusicController.instance.levelClearSong);
		UIManager.instance.UpdateDisplayMessage ("Clear");
		Invoke ("LoadScene",8f);
	}

	private void LoadScene()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void TimeScale()
	{
		Time.timeScale = 0.4f;
		Time.fixedDeltaTime = 0.04f * Time.timeScale;
	}
	
	public void ResteTimeScale()
	{
		Time.timeScale = 1f;
	}
}
