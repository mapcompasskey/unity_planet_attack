using UnityEngine;
using System.Collections;

public class MinimapController : MonoBehaviour {

	// private references
	private GameObject planet;

	void Start()
	{
		planet = transform.Find("Planet").gameObject;
	}
	
	void Update()
	{
		AlignTopRight();
	}

	void AlignTopRight()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 50, Screen.height - 50, 0));
		transform.position = new Vector2(pos.x, pos.y);
		transform.rotation = Camera.main.transform.rotation;
	}

}
