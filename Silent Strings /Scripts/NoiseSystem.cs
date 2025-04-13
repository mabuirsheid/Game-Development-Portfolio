using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    public static NoiseSystem Instance;

    [SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MakeNoise(Vector2 position, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, radius, enemyLayer);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HearNoise(position);
            }
        }
    }
}