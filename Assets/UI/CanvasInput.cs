using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasInput : MonoBehaviour
{
    public event Action OnAnyKeyDown;
    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Menu.AnyKeyDown.performed += InvokeAnyKeyDown;
        GameManager.OnAfterStateChanged += ToggleCanvasInput;
    }

    private void OnDisable()
    {
        inputActions.Menu.AnyKeyDown.performed -= InvokeAnyKeyDown;
        GameManager.OnAfterStateChanged -= ToggleCanvasInput;
    }

    private void ToggleCanvasInput(GameState state)
    {
        // canvas input should be disabled while in game to avoid constantly invoking events
        if (state == GameState.inGame) inputActions.Menu.AnyKeyDown.Disable();
        else inputActions.Menu.AnyKeyDown.Enable();
    }

    private void InvokeAnyKeyDown(InputAction.CallbackContext context)
    {
        OnAnyKeyDown?.Invoke();
    }
}