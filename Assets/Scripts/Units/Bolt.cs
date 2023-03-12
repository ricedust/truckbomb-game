using System;
using UnityEngine;

public class Bolt : MonoBehaviour, IPoolable
{
    private Action<GameObject> OnDespawn;

    public static event Action OnBoltCollected;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * GameManager.instance.gameSpeed * Time.deltaTime);
    }

    public void InitializeDespawnAction(Action<GameObject> DespawnAction)
    {
        OnDespawn = DespawnAction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Bolt entered trigger");
        if (collision.TryGetComponent(out DespawnTrigger despawnTrigger)) // despawn trigger
        {
            OnDespawn(gameObject);
        }
        if (collision.TryGetComponent(out Player player)) // player
        {
            OnBoltCollected?.Invoke(); // fire bolt collected event
            OnDespawn(gameObject);
        }
    }

    public void ResetState()
    {
        // does nothing for now since the bolt state doesn't change
        return;
    }
}
