using UnityEngine;
using System.Collections;

public class MinimapController : MonoBehaviour {

	// private references
	private GameObject player;
	private GameObject planet;
	private GameObject backgroundPlanet;

	// vectors
	private Vector2 backgroundPlanetSize;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		planet = transform.Find("Planet").gameObject;

		backgroundPlanet = GameObject.Find("Planet Background").gameObject;
		backgroundPlanetSize = backgroundPlanet.GetComponent<SpriteRenderer>().sprite.rect.size;
		//Debug.LogFormat("{0}, {1}, {2}", backgroundPlanetSize, Screen.width, Screen.height);

		//float camHalfHeight = Camera.main.orthographicSize;
		//float camHalfWidth = Camera.main.aspect * camHalfHeight; 
		//Debug.LogFormat("{0}, {1}", camHalfHeight, camHalfWidth);
	}
	
	void Update()
	{
		AlignTopRight();
	}

	void AlignTopRight()
	{
		/*
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 75, Screen.height - 75, 0));
		transform.position = new Vector2(pos.x, pos.y);
		//transform.rotation = Camera.main.transform.rotation;
		*/

		/** /
		pos = Camera.main.ScreenToWorldPoint(new Vector3(backgroundPlanetSize.x, Screen.height - backgroundPlanetSize.y, 0));
		backgroundPlanet.transform.position = new Vector3(pos.x, pos.y, 1);
		backgroundPlanet.transform.rotation = Camera.main.transform.rotation;
		/**/

		/*
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = Camera.main.aspect * camHalfHeight; 
		Bounds bounds = backgroundPlanet.GetComponent<SpriteRenderer>().bounds;
		
		// Set a new vector to the top left of the scene 
		Vector3 topLeftPosition = new Vector3(-camHalfWidth, camHalfHeight, 0) + Camera.main.transform.position; 
		
		// Offset it by the size of the object 
		topLeftPosition += new Vector3(bounds.size.x / 2,-bounds.size.y / 2, 0);
		backgroundPlanet.transform.position = topLeftPosition; 
		*/
	}

}
