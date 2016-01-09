using UnityEngine;
using System.Collections;

public class LevelManagerController : MonoBehaviour {

    // public references
    public GameObject PauseCanvas;
    public GameObject GameOverCanvas;

    // bool
    private bool gamePaused = false;
    private bool gameOver = false;

    void Start()
    {
        // hide the pause menu at start
        PauseCanvas.SetActive(false);

        // hide the game over menu at start
        GameOverCanvas.SetActive(false);
    }

    void Update()
    {
        IsGameOver();
        IsPaused();
    }

    void IsGameOver()
    {
        if (gameOver)
        {
            GameOverCanvas.SetActive(true);

            if (Input.GetButtonDown("Cancel"))
            {
                // load the first scene
                Application.LoadLevel(0);
            }
        }
    }

    void IsPaused()
    {
        if (gameOver)
        {
            return;
        }

        // listen for pause button
        if (Input.GetButtonDown("Cancel"))
        {
            gamePaused = !gamePaused;

            // is the game paused
            if (gamePaused)
            {
                PauseCanvas.SetActive(true);
                Time.timeScale = 0;
            }
            else if (!gamePaused)
            {
                PauseCanvas.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void SetGameOver()
    {
        gameOver = true;
    }

}
