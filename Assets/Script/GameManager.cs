using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject resumeButton;

    void Start()
    {
        Time.timeScale = 1f;
        resumeButton.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }
}
