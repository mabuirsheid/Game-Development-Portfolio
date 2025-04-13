using UnityEngine;

public class ShootLoop : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject target;
    private int score = 0;
    public TextMesh scoreText;

    private float moveSpeed = 5f;
    private bool isShooting = false;

    void Start()
    {
        scoreText.text = "Score: " + score;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, v, 0) * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }

        MoveTarget();
        CheckBulletCollisions();
    }

    void ShootBullet()
    {
        Vector3 bulletPos = transform.position + new Vector3(0, 1, 0);
        Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
    }

    void MoveTarget()
    {
        target.transform.Translate(Vector3.right * 2 * Time.deltaTime);
        if (target.transform.position.x > 8) target.transform.position = new Vector3(-8, 3, 0);
    }

    void CheckBulletCollisions()
    {
        Collider2D[] bullets = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var bullet in bullets)
        {
            if (bullet.CompareTag("Bullet"))
            {
                score++;
                Destroy(bullet.gameObject);
                Destroy(target.gameObject);
                scoreText.text = "Score: " + score;
            }
        }
    }
}
