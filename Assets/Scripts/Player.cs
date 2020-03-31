using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    /**
     * Player controls
     **/
    [SerializeField] public float maxSpeed = 4;
    [SerializeField] public float jumpForce = 400;
    [SerializeField] public float minHeight, maxHeight;
    [SerializeField] public int maxHealth = 10;
    [SerializeField] public string playerName;
    [SerializeField] public Sprite playerImage;
    [SerializeField] public AudioClip collisionSound, jumpSound, healthItem, deadSound;
    [SerializeField] public Weapon weapon;
    [SerializeField] public int damageCount;
    [SerializeField] public bool onGround;
    [SerializeField] public bool isDead = false;
    [SerializeField] public bool facingRight = true;
    [SerializeField] public bool holdingWeapon = false;
    [SerializeField] public bool highDamage;
    [SerializeField] public bool canAttack = true;
    [SerializeField] public bool canDamage = true;
    // The variable determines the minimum ticket between weapon attacks
    [SerializeField] public float minimumWeaponAttackTime = 0.3f;
    [SerializeField] public Vector3 highDamageForce = new Vector3(1.5f, 2.5f, 0);
    [SerializeField] public int enemyMaximumCombo = 3;

    /**
     * State variables
     * */
    private int currentHealth;
    private float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool jump = false;
    private AudioSource audioS;
    // This variable hold the last time in which the player hits with his weapon
    private float weaponAttackTime;
    private float h;
    private float z;
    private Ray ray;
    private RaycastHit hit;
    private float rayDis;

    /**
     * Initialization method
     * */
    void Start()
    {
        // Get Player components
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
        groundCheck = gameObject.transform.Find("GroundCheck");

        // Initialize speed and health when player spawns
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
    }

    // Runs on time per frame and it is better to player low reactions as jump, attack or hold an enemy
    void Update()
    {
        SetStates();
        ValidatesGround();
        HoldEnemy();
        DoJump();
        DoAttack();
    }

    // Jump the player
    private void DoJump()
    {
        // Verify if Jump button was pressed
        if (Input.GetButtonDown("Jump"))
        {
            // Verify if the player is able to jump and is not in the air or holding a weapon
            if (onGround &&
                !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") &&
                !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1 0") &&
                !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") &&
                !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3") &&
                !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage1") &&
                !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage2") &&
                holdingWeapon == false)
            {
                // Activate Jump animation
                jump = true;
            }
        }
    }

    // Hold the enemy near from the player
    private void HoldEnemy()
    {
        // Increase ray distance to detect the near enemies
        rayDis = GetComponent<CapsuleCollider>().radius + 1f;
        // Generate the origin of the ray, a little bit up from the player center
        ray.origin = transform.position + transform.up * 1f;

        // Generate Ray direction depending where the player is facing
        if (facingRight)
        {
            ray.direction = transform.right;
        }
        else if (!facingRight)
        {
            ray.direction = -transform.right;
        }

        // When user is walking, throw a ray and verify if it impacted with Enemy layer
        if (Physics.Raycast(ray, out hit, rayDis, 1 << LayerMask.NameToLayer("Enemy")) &&
            onGround &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Damage") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage1") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage2") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Voadera") &&
            !holdingWeapon
            )
        {
            // Get the enemy where the ray impacted
            Enemy holdEnemy = hit.collider.gameObject.GetComponent<Enemy>();
            // Only normal enemies can be grabbed, you cannot hold a boss
            if (holdEnemy.tag != "Boss")
            {
                // Enemy null validation
                if (holdEnemy != null)
                {
                    // Set hold for enemy
                    holdEnemy.beHold = true;
                    // Activates player's hold animation
                    anim.SetBool("HoldEnemy", true);
                }
            }
        }
        else
        {
            // Disables player's hold animation if no enemy is near from player
            anim.SetBool("HoldEnemy", false);
        }
    }

    // Verify if the player is on the ground
    private void ValidatesGround()
    {
        // Throws a line from player position to ground check empty component and verify if there is a layer between
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        // Set onGround animation
        anim.SetBool("OnGround", onGround);
    }

    // Set the dead, weapon and high damage animations if they are true
    private void SetStates()
    {
        anim.SetBool("Dead", isDead);
        anim.SetBool("Weapon", holdingWeapon);
        anim.SetBool("HighDamage", highDamage);
    }

    // Do an attack with or without weapons
    private void DoAttack()
    {
        // Validates if the user pressed the attack key
        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            // Do normal attack if the player is not holding a weapon
            if (!holdingWeapon)
            {
                Attack();
            }
            else if (holdingWeapon)
            {
                // Validates if the user is able to use the weapon again
                if (weaponAttackTime > minimumWeaponAttackTime)
                {
                    // Attack with weapon and reset the status weapon time variable
                    weaponAttackTime = 0;
                    Attack();
                }
                else
                {
                    // Increase weapon attack time
                    weaponAttackTime += Time.deltaTime;
                }
            }
        }
    }

    // Runs one or more times per frame
    private void FixedUpdate()
    {
        ReceiveCombo();
        ReceiveHighDamage();
        ReadPlayerControls();
        MovePlayer();
        FlipPlayer();
        FreezeJump();
        FreezeWhenDamage2();
        CalculateAnimationSpeed();
        MovePlayerWithRespectCamera();
        Jump();
    }

    // Functions that read horizonral and vertical control movement
    private void ReadPlayerControls()
    {
        // Read the player movement if is on ground and is not dead or receiving damage
        if (!isDead &&
            !highDamage &&
            onGround &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage1") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage2") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            // Read horizonal control
            h = Input.GetAxis("Horizontal");
            // Read depth/vertical control
            z = Input.GetAxis("Vertical");
        }
    }

    // Function that moves player in horizontal and vertical plane
    private void MovePlayer()
    {
        // Move if the player is on ground, and not on damage
        if (!isDead &&
            !highDamage &&
            onGround &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
        {
            // Move player
            rb.velocity = new Vector3(h * currentSpeed, rb.velocity.y, z * currentSpeed);
        }
    }

    // Functions that freeze the depth movement when player is jumping
    private void FreezeJump()
    {
        // Validate that player is not dead and is not on the ground
        if (!isDead && !onGround)
        {
            // Freeze depth/vertical movement
            z = 0;
        }
    }

    // Functions that freeze the player movement when the player is receing a HighDamage2
    private void FreezeWhenDamage2()
    {
        // Validate if player is not dead or is not on high damage 2
        if (!isDead && anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage2"))
        {
            // Freeze player horizontal and vertical velocity
            rb.velocity = Vector3.zero;
        }
    }

    // Functions that impulse the player to make a jump
    private void Jump()
    {
        // Validate that player is not dead and the player pressed jump button
        if (!isDead && jump)
        {
            // Unable to infite jump 
            jump = false;
            // Add a player force to jump 
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    // Function that adjust the animation velocity accordingly to player movement speed
    private void CalculateAnimationSpeed()
    {
        if (!isDead && onGround)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
        }
    }

    // Function that determines if the player need to be flipped or not
    private void FlipPlayer()
    {
        // if player is moving right and facing left and is not dead, or on high damage
        if (!isDead && h > 0 && !facingRight && !highDamage && !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage1"))
        {
            Flip();
        }
        // if player is moving left and facing right and is not dead, or on high damage
        else if (!isDead && h < 0 && facingRight && !highDamage && !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("HighDamage1"))
        {
            Flip();
        }
    }

    //TODO: Analize this code in depth
    private void MovePlayerWithRespectCamera()
    {
        if (!isDead)
        {
            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1), rb.position.y, Mathf.Clamp(rb.position.z, minHeight, maxHeight));
        }
    }

    // Validate if the player receive a combo
    private void ReceiveCombo()
    {
        // Validate if the player receives a combo from the enemy 
        if (damageCount >= enemyMaximumCombo)
        {
            // The player receives a High Damage Hit after the enemy combo
            StartCoroutine(ControlHighDamageBool());
            // Reset combo count
            damageCount = 0;
        }
    }

    // Receive high damage from enemies
    private void ReceiveHighDamage()
    {
        // Validates if the player receive a big damage
        if (highDamage && !isDead)
        {
            if (facingRight)
            {
                Vector3 inverseHighDamageForce = highDamageForce;
                // Invert the high damage force
                inverseHighDamageForce.x = -inverseHighDamageForce.x;
                // Throws the user to the right
                rb.AddForce(inverseHighDamageForce, ForceMode.Impulse);
            }
            else
            {
                // Throws the user to the left
                rb.AddForce(highDamageForce, ForceMode.Impulse);
            }
        }
    }

    // Controls what the player does after receive a combo
	public IEnumerator ControlHighDamageBool()
	{
        // Reset combo count to zero
		damageCount = 0;
        // Set the High Damage Status in order to launch the player far away
		highDamage = true;
        // Remove weapon from the player
        SetHoldingWeapon(false);

		FindObjectOfType<Weapon>().gameObject.GetComponent<SpriteRenderer>().sprite = null;
		yield return new WaitForSeconds(0.05f);
        // Reset the High Damage Status in order to stop the player from be launched again
        highDamage = false;
	}

    // Function that flips the player
	public void Flip()
	{
        // Validate if the player is not attacking
		if(!anim.GetCurrentAnimatorStateInfo(0).IsTag("DefaultAttack"))
		{
            // Invert facing variable
			facingRight = !facingRight;

            // Flip the player inverting his x scale
            Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}

    // Makes the player not to move
	void ZeroSpeed()
	{
		currentSpeed = 0;
	}

    // Reset player speed
	void ResetSpeed()
	{
		currentSpeed = maxSpeed;
	}

    // Determine the player current speed when using a weapon
	void WeaponSpeed()
	{
		currentSpeed = minimumWeaponAttackTime * maxSpeed;
	}

    // Make a respawn when the player dies
	void PlayerRespawn()
	{
        // Validates if the player still have lives
		if (FindObjectOfType<GameManager>().lives > 0) 
		{
            //TODO: Analize why this method is invoked
			StartCoroutine(CanDamageBoolControl());
            // Set dead status to false
			isDead = false;
            // Update lives in UI
			UIManager.instance.UpdateLives();
            // Reset health to max
			currentHealth = maxHealth;
            // Update health in UI
			UIManager.instance.UpdateHealth(currentHealth);
            // Restart all the player animations
			anim.Rebind();
            //TODO: Analize why this is used
            float minWidth = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 10)).x;
            // Change player position to come from the air
			transform.position = new Vector3 (minWidth, 10, -4);
		}
		else 
		{
            // Displaye Game Over message
			UIManager.instance.UpdateDisplayMessage("Game Over");
            // Destroy game manager
			Destroy(FindObjectOfType<GameManager>().gameObject,2f);
			Invoke ("LoadScene",2f);
		}
	}
	IEnumerator CanDamageBoolControl()
	{
		canDamage = false;
		yield return new WaitForSeconds(0.0f);
		for(int i = 0; i<100;i++)
		{
			yield return new WaitForSeconds(0.03f);
			this.GetComponent<SpriteRenderer>().enabled = false;
			yield return new WaitForSeconds(0.03f);
			this.GetComponent<SpriteRenderer>().enabled = true;
		}
		yield return new WaitForSeconds(0.0f);
		canDamage = true;
		yield return null;
	}
		
	
	public void TookDamage(int damage)
	{
		if(!isDead && canDamage)
		{
			if (!onGround) {
				damageCount += 4;
			} else {
				damageCount+=1;
			}

			currentHealth -= damage;
			anim.SetTrigger("HitDamage");
			UIManager.instance.UpdateHealth(currentHealth);
			PlaySong (collisionSound);
			if(currentHealth<=0)
			{
				damageCount = 0; 
				isDead = true;
				FindObjectOfType<GameManager>().lives -- ;
				holdingWeapon = false;
				FindObjectOfType<Weapon>().gameObject.GetComponent<SpriteRenderer>().sprite = null;

				PlaySong(deadSound);
				if(facingRight)
				{
					rb.AddForce(new Vector3(-3,5,0),ForceMode.Impulse);
				}
				else
				{
					rb.AddForce(new Vector3(3,5,0),ForceMode.Impulse);
				}
			}
		}
	}

	public void PlaySong(AudioClip clip)
	{
		audioS.clip = clip;
		audioS.Play ();
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Health Item"))
		{
			Destroy (other.gameObject);
			anim.SetTrigger ("Catching");
			PlaySong(healthItem);
			currentHealth = maxHealth;
			UIManager.instance.UpdateHealth(currentHealth);
		}

		if(other.CompareTag("Weapon"))
		{
			if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
			{
				anim.SetTrigger("Catching");
				holdingWeapon = true;
				WeaponItem weaponItem = other.GetComponent<PickableWeapon>().weapon;
				weapon.ActivateWeapon(weaponItem.sprite,weaponItem.color,weaponItem.durability,weaponItem.damage);
				Destroy (other.gameObject);
			}
		}
	}

	void Attack()
	{
		anim.SetTrigger ("Attack");
	}

	void PlayAttackSound()
	{
		audioS.clip = jumpSound;
		audioS.Play ();
	}

	void LoadScene()
	{
		SceneManager.LoadScene (0);
	}

    public void SetHoldingWeapon(bool flag)
    {
        holdingWeapon = flag;
    }

    public void SetHoldingWeaponToFalse()
	{
		holdingWeapon = false;
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

	public void SetCombo()
	{
		anim.SetBool ("Combo", false);
	}

	void Impulse()
	{
		
	}

	void ignoreLayer()
	{
		this.gameObject.layer = LayerMask.NameToLayer ("Enemy");
	}

	void ResetLayer()
	{
		this.gameObject.layer = LayerMask.NameToLayer ("Player");
	}

	void FalseAttack()
	{
		canAttack = false;
	}
	void TrueAttack()
	{
		canAttack = true;
	}
}
