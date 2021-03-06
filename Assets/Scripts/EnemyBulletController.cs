﻿using UnityEngine;
using System.Collections;

public class EnemyBulletController : MonoBehaviour {

	// public variables
	public GameObject impactEffect;
    public float damage = 1f;

    // private references
    private Rigidbody2D rb2d;
	
	// floats
	private float moveSpeed = 10f;
	private float killTime = 2f;
	private float killTimer = 0f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// increment the kill timer
		killTimer += Time.deltaTime;
		if (killTimer >= killTime)
		{
			Destroy(gameObject);
		}
	}
	
	void FixedUpdate()
	{
		Vector3 horizontalVelocity = Vector3.zero;
		Vector3 verticalVelocity = moveSpeed * -transform.up;
		
		// update the current velocity
		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Planet")
		{
			OnImpact();
		}
		else if (other.tag == "Player")
		{
            other.gameObject.GetComponent<PlayerController>().UpdateHealth(-damage);
            OnImpact();
		}
	}

	void OnImpact()
	{
		Instantiate(impactEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

}
