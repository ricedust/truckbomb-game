using UnityEngine;
using TMPro;

public class NumpadKey : MonoBehaviour
{
    [SerializeField] private TextMeshPro numberText;
    [SerializeField] private int number;
    public int GetNumber() => number;
    private void Awake() => numberText.text = number.ToString();
}
