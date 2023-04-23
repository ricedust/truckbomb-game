using System.Collections;
using UnityEngine;

public class PlayerStartSequence : MonoBehaviour
{
    [SerializeField] private CurveLerper playerStartAnimation;
    [SerializeField] private PlayerInvincibility playerInvincibility;
    [SerializeField] private float finalYPosition;
    [SerializeField] private float durationSeconds;

    private void OnEnable()
    {
        playerStartAnimation.LerpOnCurve(transform.position.y, finalYPosition, durationSeconds);
        StartCoroutine(AnimatePlayer());
        StartCoroutine(playerInvincibility.MakeInvincibleForSeconds(durationSeconds));
    }

    private IEnumerator AnimatePlayer()
    {
        while (playerStartAnimation.enabled)
        {
            transform.position = playerStartAnimation.currentValue * Vector2.up;
            yield return null;
        }
    }
}