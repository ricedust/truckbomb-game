using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WheelMinigame : Minigame, IPointerMoveHandler
{
    [SerializeField] private TextMeshPro spinCountText;
    [SerializeField] private int revolutionsRequired;
    [SerializeField] private List<PolygonCollider2D> zones;

    private int spinProgress;

    private int nextExpectedZone = -1;

    public void OnPointerMove(PointerEventData eventData)
    {
        int mouseZone = GetZoneAtMouse(eventData);
        if (mouseZone == -1) return;

        // if mouse enters a new zone and it's what we expect, make spin progress
        if (mouseZone == nextExpectedZone)
        {
            spinProgress++;
            spinCountText.text = (revolutionsRequired - spinProgress / 4).ToString();
            // if spin progress translates to revolutions required, win
            if (spinProgress / zones.Count >= revolutionsRequired) WinMinigame();
        }
        nextExpectedZone = GetZoneAfter(mouseZone); // update next zone
    }

    private int GetZoneAtMouse(PointerEventData eventData)
    {
        RaycastResult hit = eventData.pointerCurrentRaycast;
        if (!hit.isValid) return -1;

        PolygonCollider2D collider = hit.gameObject.GetComponent<PolygonCollider2D>();
        return zones.IndexOf(collider);
    }

    private int GetZoneAfter(int zone) => (zone + 1) % zones.Count;

    public override void ResetState()
    {
        spinProgress = 0;
        spinCountText.text = revolutionsRequired.ToString();
    }
}