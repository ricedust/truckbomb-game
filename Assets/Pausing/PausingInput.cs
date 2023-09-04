using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PausingInput : MonoBehaviour
{
    public event Action OnEscapePause;
    private InputActions inputActions;

    private void Awake() {
        inputActions = new InputActions();
    }

    private void OnEnable() {
        inputActions.Menu.Pause.performed += InvokeEscapePause;
        GameManager.OnAfterStateChanged += TogglePauseInput;
    }

    private void OnDisable() {
        inputActions.Menu.Pause.performed -= InvokeEscapePause;
        GameManager.OnAfterStateChanged -= TogglePauseInput;
    }

    // Disables input if during not inGame
    private void TogglePauseInput(GameState state) {
        if(state != GameState.inGame) {
            inputActions.Menu.Pause.Disable();
        } else {
            inputActions.Menu.Pause.Enable();
        }
    }

    private void InvokeEscapePause(InputAction.CallbackContext context) {
        OnEscapePause?.Invoke();
    }

}
