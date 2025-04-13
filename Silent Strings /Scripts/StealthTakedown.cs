using UnityEngine;

public class StealthTakedown : MonoBehaviour
{
    [SerializeField] private float takedownRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private AnimationClip takedownAnimation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, takedownRange, enemyLayer);
            foreach (var hitCollider in hitColliders)
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null && !enemy.IsAlerted())
                {
                    PerformTakedown(enemy);
                    break;
                }
            }
        }
    }

    private void PerformTakedown(Enemy enemy)
    {
        if (takedownAnimation != null && playerAnimator != null)
        {
            StartCoroutine(TakedownAnimationCoroutine(enemy));
        }
        else
        {
            enemy.TakeDamage(100); // Instant takedown
        }
    }

    private System.Collections.IEnumerator TakedownAnimationCoroutine(Enemy enemy)
    {
        playerAnimator.Play(takedownAnimation.name);
        yield return new WaitForSeconds(takedownAnimation.length);
        enemy.TakeDamage(100);
    }
}