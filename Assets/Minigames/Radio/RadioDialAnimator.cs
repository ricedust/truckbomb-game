using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RadioDialAnimator : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform radioDial;
    [SerializeField] private float dialSensitivity;

    private void OnEnable()
    {
        radioDial.localEulerAngles = Vector3.forward * 180;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("test");
        if (eventData.delta.x > 0) radioDial.localEulerAngles -= Vector3.forward * dialSensitivity;
        else radioDial.localEulerAngles += Vector3.forward * dialSensitivity;
    }
}