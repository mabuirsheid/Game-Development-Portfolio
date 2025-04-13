using UnityEngine;

public class DodgeDot : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int health = 3;
    private float spawnRate = 1f;
    private float timer = 0;
    private float movementSpeed = 5f;
    private Vector2 movement;

    public TextMesh healthText;
    public TextMesh scoreText;
    private int score = 0;

    void Start()
    {
        UpdateHealthText();
        UpdateScoreText();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        transform.Translate(movement * movementSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer > spawnRate)
        {
            timer = 0;
            SpawnEnemy();
        }

        // Check collision with enemies
        CheckCollisions();
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-7f, 7f), 6, 0);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    void CheckCollisions()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                health--;
                Destroy(hit.gameObject);  // Destroy enemy
                if (health <= 0)
                {
                    GameOver();
                }
                else
                {
                    UpdateHealthText();
                }
            }
        }
    }

    void UpdateHealthText()
    {
        healthText.text = "Health: " + health;
    }

    void UpdateScoreText()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    void GameOver()
    {
        healthText.text = "Game Over";
        scoreText.text = "Final Score: " + score;
        Time.timeScale = 0;  // Freeze game
    }
}
