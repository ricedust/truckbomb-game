using System;

public class SoundToggle : ToggleButton
{
    public static event Action<bool> OnSoundToggled;

    protected override void FireEvent()
    {
        OnSoundToggled?.Invoke(isEnabled);
    }
}