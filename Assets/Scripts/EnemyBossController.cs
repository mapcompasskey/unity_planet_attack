using UnityEngine;
using System.Collections;

public class EnemyBossController : MonoBehaviour {
	
	// public references
	public GameObject enemyBullet;
	public GameObject deathEffect;
	public EnemyBossBulletController enemyBossBullet;
	
	// private references
	private Rigidbody2D rb2d;
	private EnemyHealthManager healthManager;
	private GameObject player;
	
	// vectors
	private Vector3 horizontalVelocity = Vector3.zero;
	private Vector3 verticalVelocity = Vector3.zero;
    private Vector3 startPosition = Vector3.zero;

    // booleans
    private bool starting = true;
    private bool idling = true;
    private bool facingRight = true;
    private bool canAttack1 = false;
    private bool canTurnAround = true;

    // floats
    private float moveSpeed = 3f;
	private float horizontalAxis = 1f;
    private float turnAroundDistance = 10f;
    private float ignoreDistance = 20f;

    private float idleTime = 2f;
	private float idleTimer = 0f;
    
    private float attack1Count = 5;
    private float attack1Counter = 0;
    private float attack1Time = 0.5f;
    private float attack1Timer = 0f;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		healthManager = GetComponent<EnemyHealthManager>();
		player = GameObject.FindGameObjectWithTag("Player");

        starting = true;
        startPosition = transform.position;
        GetComponent<CircleCollider2D>().enabled = false;
    }
	
	void Update()
	{
		UpdateAction();

		// if the object has been killed
		if (healthManager.health <= 0)
		{
			Instantiate(deathEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
	
	void FixedUpdate ()
	{
        IsStarting();
        IsAttacking1();
		IsWalking();
		
		// update the current velocity
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}

	void UpdateAction()
	{
        if (starting || ! idling)
        {
            return;
        }

		idleTimer += Time.deltaTime;
		if (idleTimer >= idleTime)
		{
			idleTimer = 0f;
			idling = false;

            canAttack1 = true;

            /*
            int rnd = Random.Range(1, 2);
            switch (rnd)
			{
                case 1:
                default:
					canAttack1 = true;
                    break;
			}
            */
        }
        
	}

    void IsStarting()
    {
        if ( ! starting)
        {
            return;
        }

        horizontalVelocity = transform.right * 0f;
        verticalVelocity = transform.up * moveSpeed;

        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance > 8f)
        {
            starting = false;
            verticalVelocity = transform.up * 0f;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    void IsAttacking1()
	{
        if (starting || ! canAttack1)
        {
            return;
        }

        attack1Timer += Time.deltaTime;
        if (attack1Timer >= attack1Time)
		{
            attack1Timer = 0f;
			EnemyBossBulletController bullet;

            /*
            float startAngle = -170f;
            for (int i = 0; i < 5; i++)
            {
                float initAngle = startAngle + (i * 40);
                bullet = (EnemyBossBulletController)Instantiate(enemyBossBullet, transform.position, transform.rotation);
                bullet.OnInit(true, initAngle);
            }
            */

            float startAngle = -180f;
            for (int i = 0; i < 5; i++)
            {
                float initAngle = startAngle + (i * 40) + ((attack1Counter % 2) * 20);
                bullet = (EnemyBossBulletController)Instantiate(enemyBossBullet, transform.position, transform.rotation);
                bullet.OnInit(true, initAngle);
            }

            attack1Counter++;
            if (attack1Counter >= attack1Count)
            {
                attack1Timer = 0f;
                attack1Counter = 0f;
                canAttack1 = false;
                idling = true;
            }
		}
        
	}
	
	void IsWalking()
	{
        if (starting)
        {
            return;
        }

        horizontalAxis = 1f;
        /*
        // move towards the player if they are to far away
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < ignoreDistance)
        {
            if (canTurnAround && distance > turnAroundDistance)
            {
                // if the player is to the left
                if (player.transform.position.x < transform.position.x)
                {
                    horizontalAxis = -1f;
                }
                // else, if player is to the right
                else
                {
                    horizontalAxis = 1f;
                }
                canTurnAround = false;
            }

            if (distance < (turnAroundDistance / 2))
            {
                canTurnAround = true;
            }
        }
        */

        // if just moved right
        if (horizontalAxis > 0 && ! facingRight)
		{
			facingRight = true;
			transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
		}

		// else, if just moved left
		else if (horizontalAxis < 0 && facingRight)
		{
			facingRight = false;
			transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
		}
        
        // update local horizontal velocity
        // *transform.right returns this objects local "right" direction as a vector in world space
        horizontalVelocity = transform.right * horizontalAxis * moveSpeed;
    }
	
}
