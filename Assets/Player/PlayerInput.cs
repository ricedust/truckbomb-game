using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public event Action<int> OnPlayerMerge;
    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Merge.performed += InvokePlayerMerge;
    }

    private void OnDisable()
    {
        inputActions.Player.Merge.performed -= InvokePlayerMerge;
        inputActions.Player.Disable();
    }

    private void InvokePlayerMerge(InputAction.CallbackContext context)
    {
        OnPlayerMerge?.Invoke((int)context.ReadValue<float>());
    }
}