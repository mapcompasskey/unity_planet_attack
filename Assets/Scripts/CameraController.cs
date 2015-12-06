using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// public references
	public float verticalOffset = 0f;
	public GameObject backgroundPlanet;
	public GameObject backgroundStars;

	// private references
	private GameObject player;
	private GameObject stars;

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

		// add the stars
		AddBackgroundStars();
	}

	void AddBackgroundStars()
	{
		// get the referrences to the stars sprite
		if (backgroundStars)
		{
			// create an empty game object
			stars = new GameObject("Background Stars");
			
			// get the size of the background image
			Bounds bounds = backgroundStars.GetComponent<SpriteRenderer>().sprite.bounds;
			
			// get the size of screen
			float screenHeight = Camera.main.orthographicSize * 2f;
			float screenWidth = screenHeight / Screen.height * Screen.width;
			//Debug.LogFormat("bounds: {0}, size: {1}, wd: {2}, hg: {3}", starsBound, starsBound.size, screenWidth, screenHeight);
			
			// get the number of images needed to fill the screen
			float horizontalTiles = Mathf.Ceil(screenWidth / bounds.size.x);
			float verticalTiles = Mathf.Ceil(screenHeight / bounds.size.y);
			//Debug.LogFormat("horz: {0}, vert: {1}", horizontalTiles, verticalTiles);
			
			// fill the empty game object with images
			// *its important the "Pivot" of the sprite is set to "Top Left"
			GameObject obj;
			for (int vert = 0; vert < verticalTiles; vert++)
			{
				for (int horz = 0; horz < horizontalTiles; horz++)
				{
					obj = GameObject.Instantiate(backgroundStars, Vector3.zero, Quaternion.identity) as GameObject;
					obj.transform.parent = stars.transform;
					obj.transform.position = new Vector3(bounds.size.x * horz, bounds.size.y * -vert, 0);
				}
			}
			
			/*
			// add and stretch the image of stars across the camera
			// http://answers.unity3d.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
			stars = GameObject.Instantiate(backgroundStars, Vector3.zero, Quaternion.identity) as GameObject;
			stars.transform.localScale = new Vector3(1, 1, 1);

			Sprite spr = stars.GetComponent<SpriteRenderer>().sprite;
			float width = spr.bounds.size.x;
			float height = spr.bounds.size.y;

			float worldScreenHeight = Camera.main.orthographicSize * 2f;
			float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
			
			Vector3 xWidth = stars.transform.localScale;
			xWidth.x = worldScreenWidth / width;
			stars.transform.localScale = xWidth;
			//transform.localScale.x = worldScreenWidth / width;

			Vector3 yHeight = stars.transform.localScale;
			yHeight.y = worldScreenHeight / height;
			stars.transform.localScale = yHeight;
			//transform.localScale.y = worldScreenHeight / height;
			*/
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

		// position the stars in the top right of the screen
		if (stars)
		{
			Vector3 screenTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 30));
			stars.transform.position = screenTopLeft;
			stars.transform.rotation = transform.rotation;
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
