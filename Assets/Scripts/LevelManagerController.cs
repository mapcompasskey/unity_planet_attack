using UnityEngine;
using System.Collections;

public class LevelManagerController : MonoBehaviour {

    // public references
    //public GameObject GameOverCanvas;
    public GameObject PauseCanvas;

    // bool
    private bool isPaused = false;

    void Start()
    {
        // hide the game over menu at start
        //GameOverCanvas.SetActive(false);

        // hide the pause menu at start
        PauseCanvas.SetActive(false);
    }

    void Update()
    {
        // listen for pause button
        if (Input.GetButtonDown("Cancel"))
        {
            isPaused = !isPaused;
        }

        // is the game paused
        if (isPaused)
        {
            PauseCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!isPaused)
        {
            PauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }
    }

}
