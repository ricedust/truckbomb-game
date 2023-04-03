using System;
using System.Collections;
using UnityEngine;

public class DelayMonobehaviorStart : MonoBehaviour
{
    [SerializeField] private MonoBehaviour monoBehaviour;
    [SerializeField] private float startDelaySeconds;
    private void Awake() => StartCoroutine(Delay());

    private IEnumerator Delay()
    {
        monoBehaviour.enabled = false;
        yield return new WaitForSeconds(startDelaySeconds);
        monoBehaviour.enabled = true;
    }
}