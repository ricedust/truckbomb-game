using System.Collections;
using UnityEngine;

public class VehicleAudio : MonoBehaviour
{
    [SerializeField] private VehicleCollision vehicleCollision;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip skidSound;
    [SerializeField] private AudioSource honkSource;
    [SerializeField] private float sfxVolume;
    [SerializeField] private float chanceToSkidOnCollision;
    [SerializeField] private float chanceToHonkOnCollision;

    private bool isSoundEnabled = true;
    
    private void OnEnable()
    {
        vehicleCollision.OnCollision += PlaySFX;
        SoundToggle.OnSoundToggled += ToggleSound;
    }

    private void OnDisable()
    {
        vehicleCollision.OnCollision -= PlaySFX;
        honkSource.Stop();
    }

    private void PlaySFX()
    {
        if (vehicleCollision.collisionCount == 1)
        {
            if (Random.value < chanceToSkidOnCollision)
            {
                AudioSystem.instance.PlaySound(skidSound, transform.position, sfxVolume);
            }

            if (Random.value < chanceToHonkOnCollision && isSoundEnabled) honkSource.Play();
        }
        else
        {
            AudioSystem.instance.PlaySound(explosionSound, transform.position, sfxVolume);
            honkSource.Stop();
        }

        AudioSystem.instance.PlaySound(crashSound, transform.position, sfxVolume);
    }

    private void ToggleSound(bool isEnabled) => isSoundEnabled = isEnabled;
}