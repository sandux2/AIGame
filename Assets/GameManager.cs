using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public float gameTime = 200f;
    public float timer; // total game time in seconds
    public Text timerText; // UI text to display timer
    public GameObject LoseScreen; // UI screen for losing the game
    public GameObject PauseScreen; // UI screen for pausing the game

     void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime; // decrease timer by time since last frame
            int minutes = Mathf.FloorToInt(timer / 60); // calculate minutes
            int seconds = Mathf.FloorToInt(timer % 60); // calculate seconds
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // update UI text
        }
        else
        {
            Debug.Log("Time's up! Game Over!");
            // Implement game over logic here (e.g., show game over screen, restart level, etc.)
                Time.timeScale = 0; // pause the game
                
                LoseScreen.SetActive(true); // show lose screen
        }

        if(Input.GetKeyDown(KeyCode.Escape)) // pause menu key
        {
            Time.timeScale = 0; // pause the game
            PauseScreen.SetActive(true); // show pause screen
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = gameTime;
    }

    // Update is called once per frame

    public void ReplayButton()
    {
        Time.timeScale = 1; // resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload current scene
     }

     public void NextLevel()
     {
         Time.timeScale = 1; // resume the game
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // load next scene
     }

        public void QuitGame()
        {
            Application.Quit(); // quit the application
        }

        public void ResumeGame()
        {
            Time.timeScale = 1; // resume the game
            PauseScreen.SetActive(false); // hide pause screen
        }
    
}
