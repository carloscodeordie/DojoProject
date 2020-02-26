using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{

	public GameObject boomerang;
	public float minBoomerangTime,maxBoomerangTime;

	public Transform[] buzz = new Transform[4];
	
	public override void Start ()
	{
		base.Start ();
		Invoke ("ThrowBoomerang",Random.Range (minBoomerangTime,maxBoomerangTime));
		MusicController.instance.PlaySong (MusicController.instance.bossSong);
	}

	GameObject tempBoomerang;

	void ThrowBoomerang()
	{
		if(!isDead&&!highDamage&&Mathf.Abs(targetDistance.x)>2f)
		{
			anim.SetTrigger("Boomerang");
			for(int i = 0; i<=3; i++)
			{
				tempBoomerang = Instantiate(boomerang,buzz[i].transform.position,buzz[i].transform.rotation);
				if(facingRight)
				{
					tempBoomerang.GetComponent<Boomerang>().direction = 1;
				}
				else
				{
					tempBoomerang.GetComponent<Boomerang>().direction = -1;
				}
			}
		}
		Invoke ("ThrowBoomerang",Random.Range(minBoomerangTime,maxBoomerangTime));
	}

	void BossDefeated()
	{
		MusicController.instance.PlaySong (MusicController.instance.levelClearSong);
		UIManager.instance.UpdateDisplayMessage ("Clear");
		Invoke ("LoadScene",8f);
	}

	void LoadScene()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}

	void AddHighDamageForce()
	{
		GetComponent<Rigidbody> ().AddRelativeForce (new Vector3(0,0,0),ForceMode.Impulse);
	}
	
}
