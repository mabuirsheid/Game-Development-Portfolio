using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    private bool isInShadow = false;

    public void SetInShadow(bool inShadow)
    {
        isInShadow = inShadow;
    }

    public bool IsInShadow()
    {
        return isInShadow;
    }
}