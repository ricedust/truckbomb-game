using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioSystem : Singleton<AudioSystem> {
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundsSource;
    [SerializeField] private AudioSource ambientSource;

    [SerializeField] private AudioClip musicIntro;
    [SerializeField] private AudioClip music;

    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += StartMusic;
        GameManager.OnAfterStateChanged += StopMusic;
        SoundToggle.OnSoundToggled += ToggleSound;
        MusicToggle.OnMusicToggled += ToggleMusic;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= StartMusic;
        GameManager.OnAfterStateChanged -= StopMusic;
        SoundToggle.OnSoundToggled -= ToggleSound;
        MusicToggle.OnMusicToggled -= ToggleMusic;
    }

    private void Start()
    {
        ambientSource.Play();
    }

    public void PlayMusic(AudioClip clip) {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1)
    {
        if (!clip) return;
        soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1) {
        soundsSource.PlayOneShot(clip, vol);
    }

    private void StartMusic(GameState state)
    {
        if (state == GameState.inGame)
        {
            musicSource.loop = false;
            musicSource.clip = musicIntro;
            musicSource.Play();
            StartCoroutine(StartMusicLoopAfterIntro());
        }
    }
    
    private IEnumerator StartMusicLoopAfterIntro()
    {
        yield return new WaitUntil(() => !musicSource.isPlaying);
        musicSource.loop = true;
        musicSource.clip = music;
        musicSource.Play();
    }

    private void StopMusic(GameState state)
    {
        if (state == GameState.lose)
        {
            StopAllCoroutines();
            musicSource.Stop();
        }
    }

    private void ToggleSound(bool isEnabled)
    {
        soundsSource.mute = !isEnabled;
        ambientSource.mute = !isEnabled;
    }
    private void ToggleMusic(bool isEnabled)
    {
        musicSource.mute = !isEnabled;
    }
}