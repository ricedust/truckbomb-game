using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public enum Type
    {
        WHEEL,
        ENGINE,
        PLACEHOLDER,
        BOMB
    }

    public Type gameType;

    // [SerializeField] public string name; temporarily commented out because name is reserved for c# objects
    [SerializeField] public int damage;
    [SerializeField] public int timer;

    public abstract void UIEvent();
}
