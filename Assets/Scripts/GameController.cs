using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public Text coinText;
    public Text timerText;
    public GameObject gratitudeText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject finishPanel;

    private int coins;
    private DateTime startTime;
    [SerializeField]
    private int allCoins;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        startTime = DateTime.UtcNow;
        coinText.text = $"{coins}/{allCoins}";
        StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        while (true)
        {
            timerText.text = (startTime - DateTime.UtcNow).ToString("mm':'ss");

            yield return new WaitForEndOfFrame();
        }
    }
    public void AddCoin()
    {
        coins++;
        coinText.text = $"{coins}/{allCoins}";
    }
    public void Pause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
    public void Finish()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt($"coinslevel{levelIndex}", coins);
        PlayerPrefs.SetInt($"maxcoinslevel{levelIndex}", allCoins);
        finishPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if(SceneManager.sceneCountInBuildSettings > nextLevelIndex)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            gratitudeText.SetActive(true);
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        StopAllCoroutines();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void ChangeMusicVolume(float value)
    {
        AudioController.instance.ChangeMusicVolume(value);
    }
    public void ChangeSoundVolume(float value)
    {
        AudioController.instance.ChangeSoundVolume(value);
    }
}
