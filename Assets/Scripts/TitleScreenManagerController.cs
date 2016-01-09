using UnityEngine;
using System.Collections;

public class TitleScreenManagerController : MonoBehaviour {

	void Update ()
    {
        if (Input.anyKey)
        {
            // load the first scene
            Application.LoadLevel(1);
        }
    }
}
