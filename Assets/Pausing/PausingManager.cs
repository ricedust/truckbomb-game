using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PausingManager : MonoBehaviour
{
    
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private PausingInput pausingInput;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private Button pauseButton;

    private bool isPaused;

    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;


    private void OnEnable() {
        pausingInput.OnEscapePause += TogglePause;

        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChange(sfxVolumeSlider.value); });

        pauseButton.onClick.AddListener(TogglePause);
    }

    private void OnDisable() {
        pausingInput.OnEscapePause -= TogglePause;

        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();

        pauseButton.onClick.RemoveListener(TogglePause);
    }

    void Start() {
        pauseMenu.SetActive(false);
        isPaused = false;
    }


    private void TogglePause() {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if(isPaused) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

    private void OnMusicVolumeChange(float newValue) {
        Debug.Log("New Music Value: " + newValue);

        OnMusicVolumeChanged?.Invoke(newValue);
    }

    private void OnSFXVolumeChange(float newValue) {
        Debug.Log("New SFX Value: " + newValue);

        OnSFXVolumeChanged?.Invoke(newValue);
    }


}
