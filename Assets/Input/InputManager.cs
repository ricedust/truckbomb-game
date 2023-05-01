using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject startGameText;
    [SerializeField] private GameObject endGameText;
    [SerializeField] private Player player;
    [SerializeField] private float restartDelaySeconds;

    private bool isRestartLocked = true;

    private void OnEnable()
    {
        GameManager.OnAfterStateChanged += DelayRestart;
    }
    private void OnDisable()
    {
        GameManager.OnAfterStateChanged -= DelayRestart;
    }

    private void DelayRestart(GameState state)
    {
        if (state == GameState.lose)
        {
            StartCoroutine(WaitForRestartDelay());
        }
    }

    private IEnumerator WaitForRestartDelay()
    {
        yield return new WaitForSeconds(restartDelaySeconds);
        isRestartLocked = false;
        PlayerPrefs.SetFloat("HighScore", GameManager.instance.distanceFeet);
        PlayerPrefs.Save();
    }
    
    private void Update()
    {
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && GameManager.instance.state == GameState.starting)
        {
            GameManager.instance.ChangeState(GameState.inGame);
            
            startGameText.gameObject.SetActive(false);
            player.gameObject.SetActive(true);
        }
        else if (GameManager.instance.state == GameState.lose)
        {
            if (isRestartLocked) return;
            endGameText.gameObject.SetActive(true);
            
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}