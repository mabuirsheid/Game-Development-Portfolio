using UnityEngine;

public class LightArea : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckPlayerInLight(other, false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CheckPlayerInLight(other, true);
    }

    private void CheckPlayerInLight(Collider2D other, bool inShadow)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerStealth playerStealth = other.GetComponent<PlayerStealth>();
            if (playerStealth != null)
            {
                playerStealth.SetInShadow(inShadow);
            }
        }
    }
}