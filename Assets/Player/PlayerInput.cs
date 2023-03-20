using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private UnityEvent onMoveLeft;
    [SerializeField] private UnityEvent onMoveRight;
    private void Update()
    {
        if (GameManager.instance.state == GameState.inGame)
        {
            if (Input.GetKeyDown(KeyCode.A)) onMoveLeft?.Invoke();
            if (Input.GetKeyDown(KeyCode.D)) onMoveRight?.Invoke();
        }
    }
}
