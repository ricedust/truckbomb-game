using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour
{
    [SerializeField] private CanvasInput canvasInput;
    [SerializeField] private Transform startGameText;
    [SerializeField] private Transform endGameText;

    private void OnEnable()
    {
        canvasInput.OnAnyKeyDown += StartGame;
        GameManager.OnAfterStateChanged += ShowPostGame;
        canvasInput.OnAnyKeyDown += RestartGame;
    }

    private void OnDisable()
    {
        canvasInput.OnAnyKeyDown -= StartGame;
        GameManager.OnAfterStateChanged -= ShowPostGame;
        canvasInput.OnAnyKeyDown -= RestartGame;
    }

    private void StartGame()
    {
        // make sure the game hasn't started
        if (GameManager.instance.state != GameState.starting) return;
        
        // change to in game state
        GameManager.instance.ChangeState(GameState.inGame);
        
        // disable start game text
        startGameText.gameObject.SetActive(false);
    }

    private void ShowPostGame(GameState state)
    {
        // check that it is post game
        if (state != GameState.postGame) return;
        
        // enable post game text
        endGameText.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        // check that it is post game
        if (GameManager.instance.state != GameState.postGame) return;
        
        // reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}