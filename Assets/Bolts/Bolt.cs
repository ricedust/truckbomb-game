using System;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private PoolableObject poolableObject;

    public static event Action OnBoltCollected;

    // Update is called once per frame
    private void FixedUpdate()
    {
        float gameSpeed = GameManager.instance.gameSpeed;
        Vector3 delta = gameSpeed * Time.fixedDeltaTime * Vector2.down;
        rigidBody2D.MovePosition(transform.position + delta);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player)) // player
        {
            OnBoltCollected?.Invoke(); // fire bolt collected event
            poolableObject.Despawn();
        }
    }
}
