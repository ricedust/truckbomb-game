using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class BombMinigame : Minigame, IPointerClickHandler
{
    [SerializeField] private TextMeshPro correctCodeText;
    [SerializeField] private TextMeshPro inputCodeText;
    [SerializeField] private TextMeshPro secondsLeftText;

    [SerializeField] private float timeLimitSeconds;
    [SerializeField] private float incorrectCodeDelaySeconds;
    [SerializeField] private int codeLength;

    private string inputCode;
    private string correctCode;
    private float secondsLeft;
    private bool isNumpadLocked;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out NumpadKey numpadKey) && !isNumpadLocked)
        {
            inputCode += numpadKey.GetNumber();
            inputCodeText.text = inputCode;

            if (inputCode.Length >= codeLength) CheckInputCode();
        }
    }

    private void CheckInputCode()
    {
        if (inputCode == correctCode) Defuse();
        else
        {
            inputCodeText.text = "X";
            StartCoroutine(LockNumpadForSeconds(incorrectCodeDelaySeconds));
        }
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

