using System;
using UnityEngine;
using TMPro;

public class DistanceTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI distanceTraveledText;

    private Action<float> SetDistanceTraveled;

    public void Initialize(Action<float> SetDistanceTraveled) => this.SetDistanceTraveled = SetDistanceTraveled;
    
    private void Update()
    {
        if (GameManager.instance.gameSpeed > 0)
        {
            float currentDistance = GameManager.instance.distanceTraveledFeet;
            currentDistance += Time.deltaTime * GameManager.instance.gameSpeed * 14.7f;
            SetDistanceTraveled(currentDistance);
        }
        distanceTraveledText.text = "FEET TRAVELED: " + GameManager.instance.distanceTraveledFeet + "\nMILES TRAVELED: " + GameManager.instance.distanceTraveledFeet * 0.0001894f;
    }
}
