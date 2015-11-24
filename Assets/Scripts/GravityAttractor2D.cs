using UnityEngine;
using System.Collections;

public class GravityAttractor2D : MonoBehaviour {

	public float gravity = -10f;

	public void Attract(Transform body)
	{
		Vector2 targetDir = (body.position - transform.position).normalized;
		Vector2 bodyUp = body.up;
		
		body.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * body.rotation;
		body.GetComponent<Rigidbody2D>().AddForce(targetDir * gravity);
	}
}