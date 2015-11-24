using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class GravityBody : MonoBehaviour {

	GravityAttractor planet;
	
	void Awake()
	{
		planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
		GetComponent<Rigidbody2D>().gravityScale = 0;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
	}
	
	void FixedUpdate()
	{
		planet.Attract(transform);
	}
}
