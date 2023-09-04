using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenShaker : MonoBehaviour
{
    [SerializeField] private float shakeDurationSeconds;
    [SerializeField] private float shakeMagnitude;

    private Transform mainCamera;
    private Vector3 originalCameraPosition;

    private void OnEnable()
    {
        VehicleExploder.OnExplosion += StartShaking;
    }

    private void OnDisable()
    {
        VehicleExploder.OnExplosion -= StartShaking;
    }

    private void Awake()
    {
        mainCamera = Camera.main.transform;
        originalCameraPosition = mainCamera.position;
    }

    private void StartShaking(Vector2 explosionOrigin)
    {
        StopAllCoroutines(); // stop current shaking
        StartCoroutine(Shake(shakeDurationSeconds, shakeMagnitude));
    }

    private IEnumerator Shake(float durationSeconds, float magnitude)
    {
        float secondsElapsed = 0.0f;

        while (secondsElapsed < durationSeconds)
        {
            if(Time.timeScale != 0f) {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                mainCamera.position = new Vector3(x, y, originalCameraPosition.z);
            }
            

            secondsElapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.position = originalCameraPosition;
    }
}