using System.Collections;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
    private bool isInvincible = false;

    /// <summary>Returns true if the player meets criteria for invincibility</summary>
    public bool IsInvincible()
    {
        bool isPowerupActive = PowerupManager.instance.isPowerupActive;
        
        return isInvincible
               || isPowerupActive;
    }

    public IEnumerator MakeInvincibleForSeconds(float seconds)
    {
        isInvincible = true;
        yield return new WaitForSeconds(seconds);
        isInvincible = false;
    }
}