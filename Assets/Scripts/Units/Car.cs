using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Action<Car> DespawnAction;

    private void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime);
    }

    public void Initialize(Action<Car> DespawnAction)
    {
        this.DespawnAction = DespawnAction;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // TODO Check that collision object is the despawn trigger
        Debug.Log("Exited trigger");
        DespawnAction(this);
    }
}