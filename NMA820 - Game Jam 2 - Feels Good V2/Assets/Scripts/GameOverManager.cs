using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;

    public ScoreManager scoreManager;
    public TextMeshProUGUI scoreResultsText;

    public bool isGameOver = false;

    public GameObject backgroundMusic;
    AudioSource backgroundMusicAudioSource;

    AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSFX;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        backgroundMusicAudioSource = backgroundMusic.GetComponent<AudioSource>();

        gameOverScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver == true)
        {
            Restart();
        }
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        scoreResultsText.text = "Your waves of healing saved " + scoreManager.score.ToString() + " villagers from the curse!";

        backgroundMusicAudioSource.Stop();
        audioSource.PlayOneShot(gameOverSFX);

        isGameOver = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restart");
    }
}
