using System;
using UnityEngine;
using UnityEngine.UI;

public class Bolt : MonoBehaviour
{
    private Action<Bolt> DespawnAction;

    public static event Action OnBoltCollected;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * GameManager.instance.gameSpeed * Time.deltaTime);
    }

    public void Initialize(Action<Bolt> DespawnAction)
    {
        this.DespawnAction = DespawnAction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Bolt entered trigger");
        if (collision.TryGetComponent(out DespawnTrigger despawnTrigger)) // despawn trigger
        {
            DespawnAction(this);
        }
        if (collision.TryGetComponent(out Player player)) // player
        {
            OnBoltCollected?.Invoke(); // fire bolt collected event
            DespawnAction(this);
        }
    }
}
