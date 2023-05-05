using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class DayNightManager : MonoBehaviour
{
    [Header("Day/Night Settings")]
    [Tooltip("The Length of the day in real-time seconds")]
    [SerializeField] private float dayLength;

    [Tooltip("The current time of day, where 0 is noon, 0.5 is midnight, and 1 is noon.")]
    [Range(0f, 1f)]
    [SerializeField] private float timeOfDay;

    [Tooltip("The color gradiant fro the global light over the course of the day")]
    [SerializeField] private Gradient lightColorGradient;

    [Tooltip("The animation curve for the intesity of the global light over the course of the day")]
    [SerializeField] private AnimationCurve lightIntensityCurve;

    [Tooltip("The time of day at which the night starts")]
    [Range(0f, 1f)]
    [SerializeField] private float startNightPoint;

    [Tooltip("The time of day at which day begins")]
    [Range(0f, 1f)]
    [SerializeField] private float endNightPoint;

    public static event Action NightStart;

    public static event Action DayStart;

    private bool isNight = false;


    public Light2D globalLight;

    private void Update() {
        UpdateTimeOfDay();
        UpdateLighting(timeOfDay);
        CheckNightState();    
    }

    private void UpdateTimeOfDay() {
        float timeIncrement = Time.deltaTime / dayLength;
        timeOfDay += timeIncrement;
        if(timeOfDay > 1f) {
            timeOfDay -= 1f;
        }
    }

    public void UpdateLighting(float timeOfDay) {
        float lightIntensity = lightIntensityCurve.Evaluate(timeOfDay);
        globalLight.intensity = lightIntensity;

        Color lightColor = lightColorGradient.Evaluate(timeOfDay);
        globalLight.color = lightColor;
    }

    // Checks whether there was a change in the night state
    // First it saves the previous value of isNight
    // Checks if it is in the startNightPoint and endNightPoint range
    //      if it is, set isNight to true
    //      Otherise, set isNight to false
    // Next check if it was day and is now night
    //      if true, invoke NightStart event
    // Next check if it was nitht and is now day
    //      if true, invoke DayStart event
    private void CheckNightState() {
        bool wasNight = isNight;
        if(timeOfDay >= startNightPoint && timeOfDay < endNightPoint) {
            isNight = true;
        } else {
            isNight = false;
        }

        if(!wasNight && isNight) {
            NightStart?.Invoke();
            Debug.Log("Night Start invoked");
        } else if(wasNight && !isNight) {
            DayStart?.Invoke();
            Debug.Log("Day Start invoked");
        }
    }
}
