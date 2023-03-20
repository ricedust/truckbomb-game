using System;
using UnityEngine;

public class Bolt : MonoBehaviour, IPoolable
{
    private Action<GameObject> Despawn;

    public static event Action OnBoltCollected;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * GameManager.instance.gameSpeed * Time.deltaTime);
    }

    public void InitializeDespawnAction(Action<GameObject> Despawn) => this.Despawn = Despawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Bolt entered trigger");
        if (collision.TryGetComponent(out DespawnTrigger despawnTrigger)) // despawn trigger
        {
            Despawn(gameObject);
        }
        if (collision.TryGetComponent(out Player player)) // player
        {
            OnBoltCollected?.Invoke(); // fire bolt collected event
            Despawn(gameObject);
        }
    }

    public void ResetState()
    {
        // does nothing for now since the bolt state doesn't change
        return;
    }
}
