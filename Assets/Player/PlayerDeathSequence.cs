using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDeathSequence : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private float ragdollDrag;
    [SerializeField] private float ragdollAngularDrag;
        
    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += EnableRagdoll;
    }

    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= EnableRagdoll;
    }

    private void EnableRagdoll(GameState gameState)
    {
        if (gameState == GameState.lose)
        {
            rigidbody2d.isKinematic = false;
            rigidbody2d.drag = ragdollDrag;
            rigidbody2d.angularDrag = ragdollAngularDrag;
        }
    }
}