using UnityEngine;
using System.Collections;

public class ImpactEffectController : MonoBehaviour {
	
	void Start()
	{
		Destroy(gameObject, GetComponent<ParticleSystem>().duration);
	}

}
