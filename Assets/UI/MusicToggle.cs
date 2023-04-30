using System;
using UnityEngine;

public class MusicToggle : ToggleButton
{
    public static event Action<bool> OnMusicToggled;
        
    protected override void FireEvent()
    {
        Debug.Log("music toggled");
        OnMusicToggled?.Invoke(isEnabled);
    }
}