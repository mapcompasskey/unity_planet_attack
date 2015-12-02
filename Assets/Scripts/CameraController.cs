using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// private references
	private GameObject player;

	// vectors
	private Vector3 velocity = Vector3.zero;

	// floats
	public float verticalOffset = 0;
	private float smoothTime = 20f;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void FixedUpdate()
	{
		// *its recommended to use LateUpdate() for camera following, but I'm getting some stuttering with its Time.deltaTime value

		// move towards the player's position
		Vector3 target = player.transform.TransformPoint(new Vector3(0, verticalOffset, -20));
		transform.position = Vector3.Lerp(transform.position, target, smoothTime * Time.deltaTime);

		// move towards the player's rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, smoothTime * Time.deltaTime);
	}
}
