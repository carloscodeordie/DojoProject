using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] public float maxSpeed;
    [SerializeField] public float minHeight,maxHeight;
    [SerializeField] public float damageTime = 0.7f;
    [SerializeField] public int maxHealth;
    [SerializeField] public float attackRate = 1f;
    [SerializeField] public string enemyName;
    [SerializeField] public Sprite enemyImage;
    [SerializeField] public AudioClip collisionSound, deathSound;
    [SerializeField] public bool facingRight = false;
    [SerializeField] public Transform newTarget;
    [SerializeField] public int damageCount;
    [SerializeField] public bool highDamage;
    [SerializeField] public bool beHold = false;

    protected float currentSpeed;
    protected Rigidbody rb;
    protected Animator anim;
    // The target is the player
    protected Transform target; 
    protected Transform targetHitBox;
    protected bool isDead = false;
    protected bool damaged = false;

    private int currentHealth;
	private Transform groundCheck;
	private bool onGround;
	private float zForce;
	private float walkTimer;
	private float damageTimer;
	private float nextAttack;
	private CapsuleCollider collider;
	private AudioSource audioS;

	public virtual void Start ()
	{

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        newTarget = GameObject.Find("newTarget").transform;
        targetHitBox = GameObject.FindGameObjectWithTag("PlayerAttack").transform;
        groundCheck = transform.Find("GroundCheck");
        target = FindObjectOfType<Player>().transform;
        collider = GetComponent<CapsuleCollider>();
        audioS = GetComponent<AudioSource>();

        // Reset current speed to max
        currentSpeed = maxSpeed;
		currentHealth = maxHealth;
	}
	

	public virtual void Update ()
	{
		onGround = Physics.Linecast (transform.position,groundCheck.position,1<<LayerMask.NameToLayer("Ground"));
		anim.SetBool ("Grounded",onGround);
		anim.SetBool ("Dead",isDead);
		anim.SetBool ("BeHold",beHold);

		if (beHold) 
		{
			Invoke ("NotBeHold",0.5f);
		}

		if(!isDead)
		{
			facingRight = (target.position.x < transform.position.x) ? false : true;
			
			if (facingRight&&onGround&&!damaged&&!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy1 Attack"))
			{
				Invoke ("Right",0.5f);
			}
			else if(!facingRight&&onGround&&!damaged&&!anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy1 Attack"))
			{
				Invoke("Left",0.5f);
			}
		}

		if (damaged && !isDead) 
		{
			damageTimer += Time.deltaTime;
			if(damageTimer>= damageTime || target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Damage"))
			{
				damaged = false;
				damageTimer = 0;
				damageCount = 0;
			}
		}

		/*if (damageCount == 3)
		{
			Camera.main.gameObject.GetComponent<CameraFocusOn>().CameraFocus();
		}*/

		walkTimer += Time.deltaTime;

		if(transform.position.y < -10)
		{
			this.gameObject.SetActive(false);
			TookDamage (50);
		}
	}

	protected Vector3 newTargetDistance;
	protected Vector3 targetDistance;
	public LayerMask enemyMask;
	RaycastHit hit;
	protected float hForce;
	public virtual void FixedUpdate()
	{
		if(damageCount >= 4)
		{
			anim.SetTrigger ("HighDamage");
			highDamage = true;
			Invoke("NotHighDamage",0.05f);
			if(!isDead)
			{
				rb.AddRelativeForce(new Vector3(1.5f,0.7f,0),ForceMode.Impulse);
			}
			collider.height = 0.1f;
		} 

		Debug.DrawRay (transform.position,transform.forward*10,Color.red);

		if (!isDead&&onGround) 
		{
			newTargetDistance = newTarget.position - transform.position;
			targetDistance = target.position - transform.position;
			hForce = newTargetDistance.x / Mathf.Abs (newTargetDistance.x);
		    

			if(Physics.Raycast(transform.position,transform.forward,out hit,GetComponent<CapsuleCollider>().radius+3f,enemyMask)||
			    Physics.Raycast(transform.position,-transform.forward,out hit,GetComponent<CapsuleCollider>().radius+3f,enemyMask))
			{
				zForce = -2f*(hit.point - transform.position).normalized.z;
				//Debug.Log ("SSS");
			}
			else
			{
				if(walkTimer >= Random.Range (0.5f,0.8f))
				{
					zForce = Random.Range(-1f,1.5f);
					walkTimer = 0;
				}
			}

			if(Mathf.Abs (newTargetDistance.x) < 0.2f)
			{
				hForce = 0;
				if(newTargetDistance.z!=0)
				zForce = targetDistance.z/Mathf.Abs (newTargetDistance.z);
			}

			if(!damaged&&!highDamage && !beHold&&
				!target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HighDamage1")&&
			   !target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack2")&&
			   !target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack3")
			   )
			{
				rb.velocity = new Vector3(hForce*currentSpeed, rb.velocity.y, zForce * currentSpeed);
			}

			anim.SetFloat ("Speed",Mathf.Abs (rb.velocity.magnitude));

			if(Mathf.Abs (targetDistance.x)<=2.5f && Mathf.Abs (targetDistance.z) < 1.5f &&Time.time > nextAttack&&!damaged&&!highDamage&&
			   !target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HighDamage1")&&
			   !target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hold Enemy")&&
			   !target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack2")&&
			   !target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack3")&&
			   !target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
			{
				anim.SetTrigger("Attack");
				currentSpeed = 0;
				nextAttack = Time.time + attackRate;
			}
		}

		rb.position = new Vector3(rb.position.x,
		                          rb.position.y,
		                          Mathf.Clamp(rb.position.z,minHeight,maxHeight));
	}

	public void TookDamage(int damage)
	{
		transform.position = new Vector3 (transform.position.x,transform.position.y,target.position.z);

		if (!isDead) 
		{
			damageCount += 1;
			damaged = true;
			currentHealth -= damage;
			anim.SetTrigger ("HitDamage");

			//if(target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack1 0"))
			//{
			//	Camera.main.gameObject.GetComponent<ScreenShake>().isshakeCamera = true;
			//}

			if(target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
			{
				//Camera.main.gameObject.GetComponent<ScreenShake>().isshakeCamera = true;
				rb.AddForce (new Vector3(0,7,0),ForceMode.Impulse);
			}

			if(target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack3")&&!highDamage)
			{
				//Camera.main.gameObject.GetComponent<ScreenShake>().isshakeCamera = true;
				damageCount +=4;
			}

			if(target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Voadera"))
			{
				//Camera.main.gameObject.GetComponent<ScreenShake>().isshakeCamera = true;
				//rb.AddRelativeForce (new Vector3(-7,3,0),ForceMode.Impulse);
				damageCount += 4;
			}

			if(target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("HoldAttack"))
			{
				Camera.main.gameObject.GetComponent<ScreenShake>().isshakeCamera = true;
				//rb.AddRelativeForce (new Vector3(-7,3,0),ForceMode.Impulse);
				damageCount += 4;
			}

			if(target.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack1")) // 对其X轴
			{
				transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
			}

			PlaySong (collisionSound);
			UIManager.instance.UpdateEnemyUI(maxHealth,currentHealth,enemyName,enemyImage);
			if(currentHealth <= 0 && damageCount>=4 || target.GetComponent<Player>().holdingWeapon&&currentHealth<=0)
			{
				isDead = true;
				rb.AddRelativeForce(new Vector3(4.5f,3.5f,0),ForceMode.Impulse);//自身坐标的力
				PlaySong (deathSound);
			}
		}
	}

	public void DisableEnemy()
	{
		gameObject.SetActive (false);
	}

	void ResetSpeed()
	{
		if (onGround&&!damaged) 
		{
			currentSpeed = maxSpeed;
		}
	}

	void ZeroSpeed()
	{
		if (onGround)
		{
			currentSpeed = 0f;
		}
	}

	void NotHighDamage()
	{
		damageCount = 0;
		highDamage = false;
		collider.height = 0.7f;
	}
	void NotBeHold()
	{
		beHold = false;
	}

	void Right()
	{
		transform.eulerAngles = new Vector3 (0, 180, 0);
	}

	void Left()
	{
		transform.eulerAngles = new Vector3(0,0,0);
	}

	public void PlaySong(AudioClip clip)
	{
		audioS.clip = clip;
		audioS.Play ();
	}

	void ignoreLayer()
	{
		this.gameObject.layer = LayerMask.NameToLayer ("Player");
	}
	
	void ResetLayer()
	{
		this.gameObject.layer = LayerMask.NameToLayer ("Enemy");
	}
}
