using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLights : MonoBehaviour
{
    [SerializeField] private Light2D nightLight1;
    [SerializeField] private Light2D nightLight2;

    private void OnEnable() {
        DayNightManager.NightStart += OnNightStart;
        DayNightManager.DayStart += OnDayStart;
        nightLight1.enabled = false;
        nightLight2.enabled = false;
    }

    private void OnDisable() {
        DayNightManager.NightStart -= OnNightStart;
        DayNightManager.DayStart -= OnDayStart;
    }

    private void OnNightStart() {
        nightLight1.enabled = true;
        nightLight2.enabled = true;
    }

    private void OnDayStart() {
        nightLight1.enabled = false;
        nightLight2.enabled = false;
    }
}
