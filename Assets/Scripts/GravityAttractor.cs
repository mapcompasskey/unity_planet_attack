using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {

	// floats
	public float gravity = -10f;

	public void Attract(Transform body)
	{
		Vector2 targetDir = (body.position - transform.position).normalized;
		Vector2 bodyUp = body.up;
		Rigidbody2D rb2d = body.GetComponent<Rigidbody2D>();
		float gravityScale = body.GetComponent<GravityBody>().gravityScale;

		body.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * body.rotation;
		rb2d.AddForce(targetDir * gravity * rb2d.mass * gravityScale);
	}
}