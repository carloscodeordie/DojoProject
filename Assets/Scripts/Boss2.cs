using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss2 : Enemy
{

	public float minSpecialAttackTime,maxSpecialAttackTime;

	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		Invoke ("SpecialAttack",Random.Range(minSpecialAttackTime,maxSpecialAttackTime));
		MusicController.instance.PlaySong (MusicController.instance.bossSong);
	}

	void SpecialAttack()
	{
		if(!isDead&&!highDamage&&Mathf.Abs(targetDistance.x)>1f)
		{
			anim.SetTrigger("Boomerang");
			//Debug.Log("SpecialAttack");
		}
		Invoke ("SpecialAttack",Random.Range(minSpecialAttackTime,maxSpecialAttackTime));
	}

	void SpecialSpeed()
	{
		currentSpeed = 3 * maxSpeed;
	}

	void BossDefeated()
	{
		MusicController.instance.PlaySong (MusicController.instance.levelClearSong);
		UIManager.instance.UpdateDisplayMessage ("Beat Em Out");
		Invoke ("LoadScene",8f);
	}
	void LoadScene()
	{
		SceneManager.LoadScene ("TittleScene");
	}
}
