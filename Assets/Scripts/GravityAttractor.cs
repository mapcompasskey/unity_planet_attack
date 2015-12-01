﻿using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {

	// floats
	public float gravity = -10f;

	public void Attract(Transform body)
	{
		/*
		 * this works unless an entity spawns directly beneath the planet at rotation 180 degrees
		 * somehow the rotation on the y-axis is being flipped, causing the image to be facing flipped where it can't be seen
		 * 
		Vector2 targetDir = (body.position - transform.position).normalized;
		Vector2 bodyUp = body.up;
		Rigidbody2D rb2d = body.GetComponent<Rigidbody2D>();
		float gravityScale = body.GetComponent<GravityBody>().gravityScale;
		body.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * body.rotation;
		rb2d.AddForce(targetDir * gravity * rb2d.mass * gravityScale);
		*/

		Vector2 targetDir = (body.position - transform.position).normalized;
		float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
		Rigidbody2D rb2d = body.GetComponent<Rigidbody2D>();
		float gravityScale = body.GetComponent<GravityBody>().gravityScale;

		body.rotation = Quaternion.AngleAxis(angle, body.forward);
		rb2d.AddForce(targetDir * gravity * rb2d.mass * gravityScale);
	}
}