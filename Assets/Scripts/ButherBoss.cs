using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButherBoss : Enemy 
{
	public float minBoomerangTime,maxBoomerangTime;
	public float jumpForce;
	public AudioClip jumpSound;

	private bool jumping;
	private float jumpTime;
	private bool jump;
	private float attackCount;
	private AudioSource aS;

	public override void Start () 
	{
		base.Start ();
		aS = GetComponent<AudioSource> (); 
		//Invoke ("ComboAttack",Random.Range (minBoomerangTime,maxBoomerangTime));
		//Invoke ("JumpAttack",Random.Range (minBoomerangTime,maxBoomerangTime));
		Invoke ("SpecialAttack",Random.Range (minBoomerangTime,maxBoomerangTime));
	}

	public override void Update()
	{
		base.Update ();
		anim.SetBool ("JumpAttack", jumping);

		attackCount = Random.Range (1,4);

		if (jumping&&!isDead) 
		{
			jumpTime += Time.deltaTime;
			if(jumpTime > 1f)
			{
				jumpTime = 0;
				jumping = false;
				//jump = true;
				//Invoke ("FalseJump",0.03f);
			}
		}
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate ();
		if (jump && !isDead && !damaged &&!highDamage &&
		    anim.GetCurrentAnimatorStateInfo(0).IsName("ButcherJumpAttack1")) 
		{
			rb.AddForce(Vector3.up * jumpForce);
		}
	}

	void SpecialAttack()
	{
		if (attackCount < 2) 
		{
			ComboAttack ();
		} 
		else 
		{
			JumpAttack ();
		}
		Invoke ("SpecialAttack",Random.Range (minBoomerangTime,maxBoomerangTime));
	}

	void ComboAttack()
	{
		if(!damaged&&!highDamage&&!isDead)
		{
			//Debug.Log("Fuck You");
			anim.SetTrigger("Boomerang");
		}
		//Invoke ("SpecialAttack",Random.Range (minBoomerangTime,maxBoomerangTime));
	}

	void JumpAttack()
	{
		if(!damaged&&!highDamage&&!isDead)
		{
			jumping = true;
		}
		//Invoke ("JumpAttack",Random.Range (minBoomerangTime,maxBoomerangTime));
	}

	void FalseJump()
	{
		jump = false;
	}

	void TrueJump()
	{
		jump = true;
	}

	void ScreenShake()
	{
		Camera.main.gameObject.GetComponent<ScreenShake> ().isshakeCamera = true;
	}

	void playJumpSong()
	{
		aS.clip = jumpSound;
		aS.Play();
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
}
