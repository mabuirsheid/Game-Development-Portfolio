using UnityEngine;
using System.Collections;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private GunConfiguration gunConfig;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletReleasePoint;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip suppressorEquipSound;
    [SerializeField] private AudioClip bulletImpactSound;
    [SerializeField] private bool canBeSuppressed = false;
    [SerializeField] private bool hasSuppressor = false;

    private int currentAmmo;
    private bool isReloading = false;
    private bool isSuppressed = false;
    private AudioSource audioSource;
    private float lastShotTime;

    void Start()
    {
        SetupGun();
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < gunConfig.maxClipSize)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.T) && canBeSuppressed && hasSuppressor)
        {
            ToggleSuppressor();
        }
    }

    public void SetupGun()
    {
        if (gunConfig == null)
        {
            Debug.LogError("GunConfiguration is not assigned.");
            return;
        }
        gunConfig.gunName = gameObject.name;
        currentAmmo = gunConfig.maxClipSize;
    }

    public void Shoot(Vector3 direction)
    {
        if (isReloading || Time.time - lastShotTime < gunConfig.timeBetweenShots) return;

        if (currentAmmo <= 0)
        {
            if (gunConfig.autoReload)
            {
                StartCoroutine(Reload());
            }
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, bulletReleasePoint.position, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Setup(gunConfig, bulletImpactSound);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * gunConfig.bulletSpeed;
            }
        }

        currentAmmo--;
        lastShotTime = Time.time;

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        float noiseRadius = isSuppressed ? gunConfig.suppressedNoiseRadius : gunConfig.loudNoiseRadius;
        NoiseSystem.Instance.MakeNoise(transform.position, noiseRadius);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        if (reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(gunConfig.reloadTime);

        currentAmmo = gunConfig.maxClipSize;
        isReloading = false;
    }

    private void ToggleSuppressor()
    {
        isSuppressed = !isSuppressed;
        if (suppressorEquipSound != null)
        {
            audioSource.PlayOneShot(suppressorEquipSound);
        }
    }

    public int GetCurrentAmmo() => currentAmmo;
    public int GetMaxAmmo() => gunConfig.maxClipSize;
    public string GetGunName() => gunConfig.gunName;
    public bool IsReloading() => isReloading;
    public bool IsSuppressed() => isSuppressed;
}
