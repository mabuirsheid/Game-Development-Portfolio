using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private float lifetime;
    private bool isExplosive;
    private bool canPenetrate;
    private GameObject impactParticlePrefab;
    private AudioClip impactSound;
    [SerializeField] private GameObject player;
    [SerializeField] private Collider2D bulletCollider;
    Collider2D playerCollider;

    void Start()
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bulletCollider, playerCollider);
        Destroy(gameObject, lifetime);
    }

    public void Setup(GunConfiguration config, AudioClip bulletImpactSound)
    {
        damage = config.bulletDamage;
        lifetime = config.bulletLife;
        isExplosive = config.explosiveAmmo;
        canPenetrate = config.bulletPenetration;
        impactParticlePrefab = config.bulletImpactParticlePrefab;
        impactSound = bulletImpactSound;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Enemy>()?.TakeDamage(damage);
        }

        if (impactParticlePrefab != null)
        {
            Instantiate(impactParticlePrefab, transform.position, Quaternion.identity);
        }

        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position);
        }

        if (!canPenetrate)
        {
            Destroy(gameObject);
        }
    }
}