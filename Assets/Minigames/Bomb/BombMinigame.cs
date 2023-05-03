using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


public class BombMinigame : Minigame
{
    [SerializeField] private TextMeshProUGUI correctCodeText;
    [SerializeField] private TextMeshProUGUI inputCodeText;
    [SerializeField] private TextMeshProUGUI secondsLeftText;

    [SerializeField] private float timeLimitSeconds;
    [SerializeField] private float incorrectCodeDelaySeconds;
    [SerializeField] private int codeLength;

    private string inputCode;
    private string correctCode;
    private float secondsLeft;
    private bool isNumpadLocked;

    private void Awake()
    {
        Numkey.OnNumkeyPressed += UpdateInputCode;
    }

    private void OnDestroy()
    {
        Numkey.OnNumkeyPressed -= UpdateInputCode;
    }

    protected override void ResetState()
    {
        MinigameManager.instance.isBombOnScreen = true;
        isNumpadLocked = false;

        ClearInputCode();
        GenerateNewCode();

        secondsLeft = timeLimitSeconds;
        secondsLeftText.text = timeLimitSeconds.ToString();

        StartCoroutine(CountdownTimer());
    }

    private void GenerateNewCode()
    {
        correctCode = "";
        for (int i = 0; i < codeLength; i++)
        {
            correctCode += Random.Range(0, 10);
        }

        correctCodeText.text = correctCode;
    }

    private IEnumerator CountdownTimer()
    {
        while (secondsLeft > 0)
        {
            secondsLeft -= Time.deltaTime;
            secondsLeftText.text = ((int)secondsLeft).ToString();
            yield return null;
        }

        GameManager.instance.ChangeState(GameState.lose);
        StopAllCoroutines();
    }

    private void UpdateInputCode(int digit)
    {
        if (isNumpadLocked) return;

        inputCode += digit;
        inputCodeText.text = inputCode;
        CheckInputCode();
    }

    private void CheckInputCode()
    {
        // compare the last digits of the input and correct code
        if (inputCode[^1] != correctCode[inputCode.Length - 1])
        {
            inputCodeText.text = "XXXX";
            StartCoroutine(LockNumpadForSeconds(incorrectCodeDelaySeconds));
            return;
        }

        if (inputCode == correctCode) Defuse();
    }

    private IEnumerator LockNumpadForSeconds(float seconds)
    {
        isNumpadLocked = true;
        yield return new WaitForSeconds(seconds);
        isNumpadLocked = false;
        ClearInputCode();
    }

    private void ClearInputCode()
    {
        inputCode = inputCodeText.text = "";
    }

    private void Defuse()
    {
        StopAllCoroutines();
        MinigameManager.instance.isBombOnScreen = false;
        WinMinigame();
    }
}