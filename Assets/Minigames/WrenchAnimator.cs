using UnityEngine;
using UnityEngine.EventSystems;

public class WrenchAnimator : MonoBehaviour, IPointerMoveHandler
{
    [SerializeField] private Transform wrench;

    public void OnPointerMove(PointerEventData eventData)
    {
        // get origin of wheel in screen space
        Vector2 screenSpaceOrigin = Camera.main.WorldToScreenPoint(transform.position);
        
        // get angle between mouse and wheel origin
        float mousePivotAngle = Vector2.SignedAngle(Vector2.down, eventData.position - screenSpaceOrigin);
        
        // set wrench angle
        wrench.localEulerAngles = Vector3.forward * mousePivotAngle;
    }
}