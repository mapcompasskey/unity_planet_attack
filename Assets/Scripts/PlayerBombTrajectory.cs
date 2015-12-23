﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GravityBody))]
public class PlayerBombTrajectory : MonoBehaviour {

	// private references
	private GravityBody gravityBody;
	private GameObject[] points;

	// booleans
	private bool facingRight = true;
	
	// float
	private float speed = 20f;

	private Quaternion startRotation;

	void Start()
	{
		gravityBody = GetComponent<GravityBody>();
		gravityBody.applyForce = false;

		// create (and destroy) an empty gameobject
		GameObject simuation = new GameObject("Player Bomb Simulation");
		simuation.transform.parent = transform;
		simuation.transform.localPosition = Vector3.zero;

		// populate an array with gameobjects containing a sprite
		points = new GameObject[50];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = new GameObject("dot");
			points[i].AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dot");
			points[i].transform.localScale = new Vector3(0.1f, 0.1f, 0f);
			points[i].transform.parent = simuation.transform;
			points[i].transform.localPosition = Vector3.zero;
		}
	}

	public void Simulate(bool facingRight, float angle, Quaternion playerRotation)
	{
		float time = 0.02f;
		//float gravity = -9.81f;
		float gravity = gravityBody.gravity;
		float gravityScale = gravityBody.gravityScale;
		Quaternion rotation = Quaternion.identity;
		Vector3 position = transform.position;
		Vector3 horizontalVelocity = Vector3.zero;
		Vector3 verticalVelocity = Vector3.zero;
		
		float timePassed = 0f;
		float moveSpeed = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
		float jumpSpeed = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

		// start 1 unit away from the player
		Vector3 horizontalPosition = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1);
		Vector3 verticalPosition = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad);
		position += playerRotation * (horizontalPosition + verticalPosition);
		
		for (int i = 0; i < points.Length; i++)
		{
			timePassed += time;
			points[i].transform.position = position;
			
			//Vector2 targetDir = (position - origin).normalized;
			//float degrees = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
			//rotation = Quaternion.Euler(0, 0, degrees);
			rotation = gravityBody.GetRotation(position);
			
			horizontalVelocity = points[i].transform.right * (facingRight ? 1 : -1) * moveSpeed;
			verticalVelocity = points[i].transform.up * (jumpSpeed + (gravity * gravityScale * timePassed));
			
			points[i].transform.position += rotation * (horizontalVelocity + verticalVelocity) * time;
			
			position = points[i].transform.position;
		}
	}

}
