using UnityEngine;
using System.Collections;

[RequireComponent (typeof (GravityBody))]
public class PlayerBombTrajectory : MonoBehaviour {

	// private references
	private GravityBody gravityBody;
	private GameObject[] points;
	
	// float
	private float speed = 20f;

	void Start()
	{
		gravityBody = GetComponent<GravityBody>();
		gravityBody.applyForce = false;

		// populate an array with gameobjects containing a sprite
		points = new GameObject[50];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = new GameObject("dot");
			points[i].AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dot");
            points[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.25f);
            points[i].transform.localScale = new Vector3(0.1f, 0.1f, 0f);
			points[i].transform.parent = transform;
			points[i].transform.localPosition = Vector3.zero;
		}
	}

	public void Simulate(bool facingRight, float angle, Quaternion playerRotation)
	{
		float time = 0.02f;
		float gravity = gravityBody.gravity;
		float gravityScale = gravityBody.gravityScale;
		Quaternion rotation = Quaternion.identity;
		Vector3 position = transform.position;
		
		float timePassed = 0f;
		float moveSpeed = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
		float jumpSpeed = speed * Mathf.Sin(angle * Mathf.Deg2Rad);

		// start 1 unit away from the player
		Vector3 horizontalVelocity = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * (facingRight ? 1 : -1);
		Vector3 verticalVelocity = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad);
		position += playerRotation * (horizontalVelocity + verticalVelocity);
		
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
