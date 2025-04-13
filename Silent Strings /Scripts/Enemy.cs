using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int health = 100;
    [SerializeField] private float viewRadius = 5f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private float hearingRadius = 7f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float waitTime = 2f;

    private Transform player;
    private bool isAlerted = false;
    private int currentPatrolIndex = 0;
    private Vector3 lastKnownPlayerPosition;
    private EnemyState currentState = EnemyState.Patrolling;

    private enum EnemyState
    {
        Patrolling,
        Investigating,
        Chasing
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(PatrolCoroutine());
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (CanSeePlayer())
                {
                    currentState = EnemyState.Chasing;
                    isAlerted = true;
                }
                break;
            case EnemyState.Investigating:
                MoveTowards(lastKnownPlayerPosition);
                if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 0.1f)
                {
                    currentState = EnemyState.Patrolling;
                    StartCoroutine(PatrolCoroutine());
                }
                break;
            case EnemyState.Chasing:
                if (CanSeePlayer())
                {
                    MoveTowards(player.position);
                }
                else
                {
                    currentState = EnemyState.Investigating;
                    lastKnownPlayerPosition = player.position;
                }
                break;
        }
    }

    private bool CanSeePlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= viewRadius)
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, dirToPlayer) < viewAngle / 2)
            {
                if (!Physics2D.Raycast(transform.position, dirToPlayer, viewRadius, obstacleLayer))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void HearNoise(Vector2 noisePosition)
    {
        if (Vector2.Distance(transform.position, noisePosition) <= hearingRadius)
        {
            StopAllCoroutines();
            currentState = EnemyState.Investigating;
            lastKnownPlayerPosition = noisePosition;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator PatrolCoroutine()
    {
        while (currentState == EnemyState.Patrolling)
        {
            if (patrolPoints.Length == 0) yield break;

            Vector3 nextPoint = patrolPoints[currentPatrolIndex].position;
            while (Vector3.Distance(transform.position, nextPoint) > 0.1f)
            {
                MoveTowards(nextPoint);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        isAlerted = true;
        currentState = EnemyState.Chasing;
        if (health <= 0)
        {
            Die();
        }
    }

 private void Die()
    {
        uiManager.UpdateKillCount();
        uiManager.ShowMoreText();
        Destroy(gameObject);
    }

    public bool IsAlerted()
    {
        return isAlerted;
    }
}