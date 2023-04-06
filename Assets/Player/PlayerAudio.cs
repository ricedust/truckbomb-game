using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource engineSource;
    [SerializeField] private AudioClip skidSound;
    [SerializeField] private float skidVolume;
    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += StopEngine;
        engineSource.Play();
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= StopEngine;
    }

    private void StopEngine(GameState state)
    {
        if (state == GameState.lose)
        {
            engineSource.Stop();
            AudioSystem.instance.PlaySound(skidSound, transform.position, skidVolume);
        }
    }
}