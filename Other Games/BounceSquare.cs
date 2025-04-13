using UnityEngine;

public class BounceSquare : MonoBehaviour
{
    public Vector2 speed = new Vector2(5, 5);
    private Vector2 direction = Vector2.one;
    private int score = 0;
    private float powerUpTimer = 0;
    private bool hasPowerUp = false;

    public float powerUpDuration = 5f;
    public GameObject powerUpPrefab;
    public TextMesh scoreText;

    void Start()
    {
        SpawnPowerUp();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos += (Vector3)(speed * direction * Time.deltaTime);
        transform.position = pos;

        // Bounce logic
        if (pos.x < -8 || pos.x > 8) direction.x *= -1;
        if (pos.y < -4 || pos.y > 4) direction.y *= -1;

        // Power-up timer
        if (hasPowerUp)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0)
            {
                hasPowerUp = false;
                speed = new Vector2(5, 5);  // Reset speed
                scoreText.color = Color.white;
            }
        }

        // Check for power-up collision
        if (hasPowerUp && pos.x > 2 && pos.x < 3 && pos.y > 2 && pos.y < 3)
        {
            score += 10;
            scoreText.text = "Score: " + score;
            hasPowerUp = false;
        }

        // Spawn new power-up if needed
        if (!hasPowerUp)
        {
            powerUpTimer = Random.Range(3f, 10f);
            SpawnPowerUp();
        }
    }

    private void SpawnPowerUp()
    {
        float randomX = Random.Range(-7f, 7f);
        float randomY = Random.Range(-3f, 3f);
        Vector3 powerUpPosition = new Vector3(randomX, randomY, 0);

        Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity);
        hasPowerUp = true;
    }
}
