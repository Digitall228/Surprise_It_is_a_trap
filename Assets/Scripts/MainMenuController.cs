using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject controlsPanel;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public Text[] gemsText;
    public void Play()
    {
        for (int i = 0; i < gemsText.Length; i++)
        {
            if(PlayerPrefs.HasKey($"coinslevel{i+1}"))
            {
                gemsText[i].text = $"Gems: {PlayerPrefs.GetInt($"coinslevel{i + 1}")}/{PlayerPrefs.GetInt($"maxcoinslevel{i+1}")}";
            }
        }

        levelPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }
    public void Back()
    {
        levelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }
    public void OpenControls()
    {
        levelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    public void OpenSettings()
    {
        levelPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void SetMusicVolume(float value)
    {
        AudioController.instance.ChangeMusicVolume(value);
    }
    public void SetSoundVolume(float value)
    {
        AudioController.instance.ChangeSoundVolume(value);
    }
    public void ToggleMotionBlur(bool value)
    {
        PostProcessingController.instance.motionBlur = value;
        PostProcessingController.instance.SetEffects();
    }
    public void ToggleBloom(bool value)
    {
        PostProcessingController.instance.bloom = value;
        PostProcessingController.instance.SetEffects();
    }
    public void ToggleLensDistortion(bool value)
    {
        PostProcessingController.instance.lensDistortion = value;
        PostProcessingController.instance.SetEffects();
    }
    public void ToggleVignette(bool value)
    {
        PostProcessingController.instance.vignette = value;
        PostProcessingController.instance.SetEffects();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
