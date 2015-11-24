using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class GravityBody2D : MonoBehaviour {

	GravityAttractor2D planet;
	
	void Awake()
	{
		planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor2D>();
		GetComponent<Rigidbody2D>().gravityScale = 0;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
	}
	
	void FixedUpdate()
	{
		planet.Attract(transform);
	}
}
