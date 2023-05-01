using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource engineSource;
    [SerializeField] private AudioClip skidSound;
    [SerializeField] private SoundToggle soundToggle;
    [SerializeField] private float skidVolume;
    
    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += StopEngine;
        SoundToggle.OnSoundToggled += ToggleSound;
        engineSource.mute = !soundToggle.isEnabled;
        engineSource.Play();
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= StopEngine;
        SoundToggle.OnSoundToggled -= ToggleSound;
    }

    private void StopEngine(GameState state)
    {
        if (state == GameState.lose)
        {
            engineSource.Stop();
            AudioSystem.instance.PlaySound(skidSound, transform.position, skidVolume);
        }
    }
    
    private void ToggleSound(bool isEnabled) => engineSource.mute = !isEnabled;
}