using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Script References
    private PlayerMovement pm;
    private SpriteContainer sc;
    GameObject StandingSprite;
    SpriteRenderer StandingSpriteActive;

    // Movement Variables
    private float speed;
    private bool moving = false;
    private bool movingUp = false;
    private bool movingDown = false;
    private bool movingLeft = false;
    private bool movingRight = false;

    // Variables for LegRotation
    private float legZ = 0;
    private Quaternion legRotation;
    public float rotationSpeed = 5.0f;

    // Animation Arrays
    private Sprite[] walking, legSpr;
    private int counter = 0, legCount = 0;
    private int nOfSprites;
    private float timer = 0.0f, legSwitchTimer = 0.0f;

    // Serialized Sprites
    [SerializeField] private SpriteRenderer torso;
    [SerializeField] private SpriteRenderer legs;

    public bool IsMoving { get; private set; }

    void Start()
    {
        // Ensure pm and sc are properly initialized
        pm = GetComponent<PlayerMovement>();
        sc = GameObject.Find("GameController").GetComponent<SpriteContainer>();
        StandingSprite = GameObject.Find("StandingSprite");
        StandingSpriteActive = StandingSprite.GetComponent<SpriteRenderer>();
        StandingSpriteActive.enabled = true;

        if (pm == null)
        {
            Debug.LogError("PlayerMovement script is missing!");
            return;
        }

        if (sc == null)
        {
            Debug.LogError("SpriteContainer script is missing!");
            return;
        }

        // Initialize animation arrays
        walking = sc.getPlayerUnarmedWalk();
        legSpr = sc.getPlayerLegs();

        nOfSprites = legSpr.Length;
        Debug.Log("The number of leg sprites is: " + nOfSprites);

        // Get movement & directional objects from PlayerMovement
        UpdateMovementVariables();

        // Initialize leg rotation
        legRotation = legs.transform.rotation;
    }

    void Update()
    {
        // Continuously update movement variables
        UpdateMovementVariables();

        // Call animation and rotation methods in Update
        AnimateTorso();
        AnimateLegs();
        RotateLegs();

        // Update IsMoving property
        IsMoving = moving;
    }

    void UpdateMovementVariables()
    {
        moving = pm.moving;
        movingUp = pm.movingUp;
        movingDown = pm.movingDown;
        movingLeft = pm.movingLeft;
        movingRight = pm.movingRight;
        speed = pm.speed;
    }

    void AnimateTorso()
    {
        if (moving)
        {
            StandingSpriteActive.enabled = false;
            torso.sprite = walking[counter];
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                counter = (counter + 1) % walking.Length;
                timer = 0.1f;
            }
        }
        else
        {
            // Don't enable StandingSpriteActive here, it's handled in GunPickupAndThrow
            torso.sprite = walking[0]; // Reset to the first sprite when not moving
        }
    }

    void AnimateLegs()
    {
        if (moving)
        {
            legs.sprite = legSpr[legCount];
            legSwitchTimer -= Time.deltaTime;
            if (legSwitchTimer <= 0)
            {
                legCount = (legCount + 1) % legSpr.Length;
                legSwitchTimer = nOfSprites / (speed * 55);
            }
        }
        else
        {
            legs.sprite = legSpr[0]; // Reset to the first sprite when not moving
            legs.sprite = null;
        }
    }

    void RotateLegs()
    {
        if (moving)
        {
            // Calculate the target rotation based on movement direction
            if (Input.GetKey(KeyCode.W))
            {
                legZ = 0.0f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                legZ = 180.0f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                legZ = 90.0f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                legZ = -90.0f;
            }
            
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                legZ = 45.0f;
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
            {
                legZ = -45.0f;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
            {
                legZ = 135.0f;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                legZ = -135.0f;
            }

            // Smoothly interpolate towards the target rotation
            legRotation = Quaternion.Euler(0, 0, legZ);
            legs.transform.rotation = Quaternion.Lerp(legs.transform.rotation, legRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // When not moving, reset the legs' rotation
            legs.transform.rotation = Quaternion.identity;
        }
    }
}