using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float carCollisionForce;

    public static event Action OnCarCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Car car))
        {
            PushAway(car);
            OnCarCollision?.Invoke();
        }
    }

    private void PushAway(Car car)
    {
        Vector2 angleOfCollision = (car.transform.position - transform.position);
        angleOfCollision.Normalize();

        car.ApplyForce(angleOfCollision * carCollisionForce);
    }
}
