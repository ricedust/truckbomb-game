using System.Collections;
using TMPro;
using UnityEngine;

public class TextBlinker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private float blinkIntervalSeconds;

    private void OnEnable()
    {
        StartCoroutine(BlinkText());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    private IEnumerator BlinkText()
    {
        while (true)
        {
            textMesh.alpha = textMesh.alpha == 0 ? 255 : 0;
            yield return new WaitForSeconds(blinkIntervalSeconds);
        }
    }
}