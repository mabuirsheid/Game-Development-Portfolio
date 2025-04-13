using UnityEngine;

public class GunPickupAndThrow : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private SpriteRenderer standingSpriteRenderer;
    [SerializeField] private Transform bulletReleasePoint;
    private SpriteRenderer torsoRenderer;

    [SerializeField] private float equipRange = 1f;

    [Header("Throw Settings")]
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwDrag = 2f;

    private GameObject currentWeapon;
    private SpriteRenderer currentWeaponSpriteRenderer;
    private GunShooting currentGunShooting;

    public bool EquippedWeapon { get; private set; }

    private void Start()
    {
        if (playerAnimation == null)
        {
            Debug.LogError("PlayerAnimation script is missing!");
            return;
        }

        standingSpriteRenderer = player.transform.Find("StandingSprite").GetComponent<SpriteRenderer>();
        torsoRenderer = playerAnimation.transform.Find("PlayerTorso").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (EquippedWeapon)
            {
                ThrowWeapon();
            }
            else
            {
                PickupWeapon();
            }
        }

        if (EquippedWeapon && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        UpdateStandingSpriteVisibility();
    }

    private void UpdateStandingSpriteVisibility()
    {
        standingSpriteRenderer.enabled = !EquippedWeapon && !playerAnimation.IsMoving;
    }

    private void PickupWeapon()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, equipRange);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Weapon"))
            {
                EquipWeapon(collider.gameObject);
                break;
            }
        }
    }

    private void EquipWeapon(GameObject weapon)
    {
        currentWeapon = weapon;

        string weaponSpriteName = weapon.name + "Sprite";
        Transform weaponSpriteTransform = player.transform.Find("WeaponSprites/" + weaponSpriteName);

        if (weaponSpriteTransform == null)
        {
            Debug.LogError($"Weapon sprite {weaponSpriteName} not found!");
            return;
        }

        currentWeaponSpriteRenderer = weaponSpriteTransform.GetComponent<SpriteRenderer>();
        currentGunShooting = weapon.GetComponent<GunShooting>();

        if (currentGunShooting == null)
        {
            Debug.LogError($"GunShooting component not found on weapon: {weapon.name}");
            return;
        }

        uiManager.UpdateGunName(currentGunShooting.GetGunName());
        uiManager.UpdateAmmo(currentGunShooting.GetCurrentAmmo(), currentGunShooting.GetMaxAmmo());

        weapon.SetActive(false);

        torsoRenderer.enabled = false;
        currentWeaponSpriteRenderer.enabled = true;

        EquippedWeapon = true;

        Debug.Log($"Equipped weapon: {weapon.name}");
    }

    private void ThrowWeapon()
    {
        currentWeapon.transform.position = player.transform.position;
        currentWeapon.SetActive(true);

        Rigidbody2D rb = currentWeapon.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = currentWeapon.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }

        Vector2 throwDirection = player.transform.up;
        rb.velocity = throwDirection * throwForce;
        rb.drag = throwDrag;

        currentWeaponSpriteRenderer.enabled = false;
        torsoRenderer.enabled = true;

        currentWeapon = null;
        currentGunShooting = null;
        EquippedWeapon = false;

        Debug.Log("Weapon thrown.");
    }

    private void Shoot()
    {
        if (currentGunShooting != null && !currentGunShooting.IsReloading())
        {
            currentGunShooting.Shoot(transform.up);
            uiManager.UpdateAmmo(currentGunShooting.GetCurrentAmmo(), currentGunShooting.GetMaxAmmo());
        }
    }
}