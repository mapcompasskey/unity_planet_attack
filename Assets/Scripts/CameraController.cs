using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// public references
	public float verticalOffset = 0f;
	public GameObject backgroundPlanet;
	public GameObject backgroundStars;

	// private references
	private GameObject player;
	private GameObject bgStars;

	// vectors
	private Vector3 planetOffset;
	private Vector3 starsOffset;

	// floats
	private float smoothTime = 20f;
	private float cameraPositionZ = 0f;

	void Start()
	{
		cameraPositionZ = transform.position.z;

		player = GameObject.FindGameObjectWithTag("Player");

		// get the referrences to background planet
		if (backgroundPlanet)
		{
			Bounds planetBounds = backgroundPlanet.GetComponent<SpriteRenderer>().sprite.bounds;
			Vector2 planetSize = new Vector2(planetBounds.size.x * backgroundPlanet.transform.localScale.x, planetBounds.size.y * backgroundPlanet.transform.localScale.y);
			planetOffset = new Vector3(planetSize.x / 2, -planetSize.y / 2, 0);
			planetOffset += new Vector3(-1, -6, 0);
		}

		// get the referrences to the stars
		if (backgroundStars)
		{
			/*
			bgStars = GameObject.Instantiate(backgroundStars, Vector3.zero, Quaternion.identity) as GameObject;
			Bounds starsBound = backgroundStars.GetComponent<SpriteRenderer>().sprite.bounds;
			Vector2 planetSize = new Vector2(starsBound.size.x, starsBound.size.y);
			starsOffset = new Vector3(planetSize.x / 2, -planetSize.y / 2, 0);

			float wd = backgroundStars.GetComponent<SpriteRenderer>().sprite.rect.width;
			float ppu = backgroundStars.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

			Debug.LogFormat("{0}, {1}, {2}, {3}, {4}, {5}", wd, ppu, wd / ppu, Camera.main.pixelWidth, Camera.main.orthographicSize, wd / Camera.main.orthographicSize);
			*/

			// add and stretch the image of stars across the camera
			// http://answers.unity3d.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
			bgStars = GameObject.Instantiate(backgroundStars, Vector3.zero, Quaternion.identity) as GameObject;
			bgStars.transform.localScale = new Vector3(1, 1, 1);

			Sprite spr = bgStars.GetComponent<SpriteRenderer>().sprite;
			float width = spr.bounds.size.x;
			float height = spr.bounds.size.y;

			float worldScreenHeight = Camera.main.orthographicSize * 2f;
			float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
			
			Vector3 xWidth = bgStars.transform.localScale;
			xWidth.x = worldScreenWidth / width;
			bgStars.transform.localScale = xWidth;
			//transform.localScale.x = worldScreenWidth / width;

			Vector3 yHeight = bgStars.transform.localScale;
			yHeight.y = worldScreenHeight / height;
			bgStars.transform.localScale = yHeight;
			//transform.localScale.y = worldScreenHeight / height;
			
		}
	}
	
	void Update()
	{
		// position the planet in the top left of the screen
		if (backgroundPlanet)
		{
			Vector3 screenTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 29));
			backgroundPlanet.transform.position = screenTopLeft + (transform.rotation * planetOffset);
			backgroundPlanet.transform.rotation = transform.rotation;
		}

		// position the stars in the center of the screen
		if (bgStars)
		{
			Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 30));
			bgStars.transform.position = screenCenter;
			bgStars.transform.rotation = transform.rotation;
		}
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
