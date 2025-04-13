using UnityEngine;

[System.Serializable]
public class GunConfiguration
{
    public string gunName = "DefaultGun";

    [Header("Ammo Settings")]
    public bool useAmmoLimit = true;
    public bool useDelayBetweenShots = true;
    public float timeBetweenShots = 0.5f;
    public bool autoReload = true;
    public int maxClipSize = 15;
    public float reloadTime = 2f;

    [Header("Weapon Type")]
    public WeaponType weaponType = WeaponType.Single;

    [Header("Bullet Settings")]
    public float bulletSpeed = 20f;
    public int bulletDamage = 10;
    public float bulletLife = 2.5f;
    public bool explosiveAmmo = false;
    public bool bulletPenetration = false;
    public GameObject bulletImpactParticlePrefab;

    public enum WeaponType { Single, Burst, Automatic }

    [Header("Sound Settings")]
    public float suppressedNoiseRadius = 5f;
    public float loudNoiseRadius = 15f;
}
