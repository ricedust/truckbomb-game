using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelGame : Minigame
{
    // Constructor for Wheel Minigame (sets name, damage, timer, ...)
    public WheelGame()
    {
        // name = "Wheel Minigame"; this name isn't legal, Unity gets mad, see Minigame.cs
        damage = 1;
        timer = 5;
        gameType = Type.WHEEL;
    }
    override public void UIEvent()
    {
    }
}