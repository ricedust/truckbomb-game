using System;
using TMPro;
using UnityEngine;

public class DistanceText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;
    
    private const float feetToMiles = 0.0001894f;
    private void Update()
    {
        if (GameManager.instance.gameSpeed > 0)
        {
            float distanceMiles = GameManager.instance.distanceFeet * feetToMiles;
            distanceText.text = distanceMiles.ToString("F3") + " miles";
        }
        else
        {
            float highScoreMiles = PlayerPrefs.GetFloat("HighScore", 0) * feetToMiles;
            distanceText.text = "Best: " + highScoreMiles.ToString("F3") + " miles";
        }
    }
}