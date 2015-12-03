using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// private references
	private GameObject player;
	private GameObject backgroundPlanet;

	// vectors
	private Vector3 planetOffset;

	// floats
	public float verticalOffset = 0f;
	private float smoothTime = 20f;
	private float cameraPositionZ = 0f;

	void Start()
	{
		cameraPositionZ = transform.position.z;

		player = GameObject.FindGameObjectWithTag("Player");

		// get the referrences to background planet
		backgroundPlanet = transform.Find("Background Planet").gameObject;
		Bounds planetBounds = backgroundPlanet.GetComponent<SpriteRenderer>().sprite.bounds;
		Vector2 planetSize = new Vector2(planetBounds.size.x * backgroundPlanet.transform.localScale.x, planetBounds.size.y * backgroundPlanet.transform.localScale.y);
		planetOffset = new Vector3(planetSize.x / 2, -planetSize.y / 2, 0);
		planetOffset += new Vector3(-1, -1, 0);
	}

	void Update()
	{
		// position the planet in the top left of the screen
		Vector3 screenTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 1));
		backgroundPlanet.transform.position = screenTopLeft + (transform.rotation * planetOffset);
	}
	
	void FixedUpdate()
	{
		// *its recommended to use LateUpdate() for camera following, but I'm getting some stuttering with its Time.deltaTime value

		// move towards the player's position
		Vector3 target = player.transform.TransformPoint(new Vector3(0, verticalOffset, cameraPositionZ));
		transform.position = Vector3.Lerp(transform.position, target, smoothTime * Time.deltaTime);

		// move towards the player's rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, smoothTime * Time.deltaTime);
	}
}
