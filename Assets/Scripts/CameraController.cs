using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// private references
	private GameObject player;
	private GameObject minimap;
	private GameObject backgroundPlanet;

	// vectors
	private Vector3 minimapOffset;
	private Vector3 planetOffset;

	// floats
	public float verticalOffset = 0;
	private float smoothTime = 20f;
	private float cameraZ = 0f;

	void Start()
	{
		cameraZ = transform.position.z;

		player = GameObject.FindGameObjectWithTag("Player");

		// get the referrences to the minimap
		minimap = transform.Find("Minimap").gameObject;
		//Bounds minimapBounds = minimap.transform.Find("Planet").GetComponent<SpriteRenderer>().sprite.bounds;
		//Vector2 minimapSize = new Vector2(minimapBounds.size.x * minimap.transform.localScale.x, minimapBounds.size.y * minimap.transform.localScale.y);
		//minimapOffset = new Vector3(-minimapSize.x / 2, -minimapSize.y / 2, 0);
		//minimapOffset += new Vector3(-1, -1, 0);
		minimapOffset = new Vector3(-2.25f, -2.25f, 0);

		// get the referrences to background planet
		backgroundPlanet = transform.Find("Background Planet").gameObject;
		Bounds planetBounds = backgroundPlanet.GetComponent<SpriteRenderer>().sprite.bounds;
		Vector2 planetSize = new Vector2(planetBounds.size.x * backgroundPlanet.transform.localScale.x, planetBounds.size.y * backgroundPlanet.transform.localScale.y);
		planetOffset = new Vector3(planetSize.x / 2, -planetSize.y / 2, 0);
		planetOffset += new Vector3(-1, -1, 0);
	}

	void Update()
	{
		// position the minimap in the top right of the screen
		Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1));
		minimap.transform.position = screenTopRight + (transform.rotation * minimapOffset);

		// position the planet in the top left of the screen
		Vector3 screenTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 1));
		backgroundPlanet.transform.position = screenTopLeft + (transform.rotation * planetOffset);
	}
	
	void FixedUpdate()
	{
		// *its recommended to use LateUpdate() for camera following, but I'm getting some stuttering with its Time.deltaTime value

		// move towards the player's position
		Vector3 target = player.transform.TransformPoint(new Vector3(0, verticalOffset, cameraZ));
		transform.position = Vector3.Lerp(transform.position, target, smoothTime * Time.deltaTime);

		// move towards the player's rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, smoothTime * Time.deltaTime);
	}
}
