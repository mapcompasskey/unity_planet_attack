using UnityEngine;
using System.Collections;

public class ParticleSystemController : MonoBehaviour {
	
	void Start()
	{
		Destroy(gameObject, GetComponent<ParticleSystem>().duration);
	}

}
