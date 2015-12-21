using UnityEngine;
using System.Collections;

//[RequireComponent (typeof (Rigidbody2D))]
public class GravityBody : MonoBehaviour {

	// public variables
	public bool applyForce = true;
	public float gravityScale = 1f;

	// private references
	private GravityAttractor planet;
	
	[HideInInspector] public float gravity = 0f;
	[HideInInspector] public Quaternion rotation = Quaternion.identity;

	void Awake()
	{
		planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravityAttractor>();
		if (GetComponent<Rigidbody2D>())
		{
			GetComponent<Rigidbody2D>().gravityScale = 0;
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}
	
	void FixedUpdate()
	{
		planet.Attract(transform);
	}
}
