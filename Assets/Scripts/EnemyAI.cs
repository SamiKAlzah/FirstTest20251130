using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform patrolPointA;
    [SerializeField] private Transform patrolPointB;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float patrolWaitTime = 2f;

    private NavMeshAgent navMeshAgent;
    private Transform currentPatrolTarget;
    private bool isChasing = false;
    private float patrolWaitCounter = 0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Auto-find player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Set initial patrol target
        if (patrolPointA != null)
        {
            currentPatrolTarget = patrolPointA;
        }
    }

    void Update()
    {
        if (player != null)
        {
          
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check if player is in detection range
            if (distanceToPlayer < detectionRange)
            {
                isChasing = true;
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                isChasing = false;
                Patrol();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // If not chasing, move between patrol points
        if (currentPatrolTarget == null)
        {
            return;
        }
        // Calculate distance to current patrol target
        float distanceToTarget = Vector3.Distance(transform.position, currentPatrolTarget.position);

        // If reached patrol point, wait and switch target
        if (distanceToTarget < 1f)
        {
            // Wait at patrol point
            patrolWaitCounter += Time.deltaTime;
            // After waiting, switch to the other patrol point
            if (patrolWaitCounter >= patrolWaitTime)
            {
                SwitchPatrolTarget();
                patrolWaitCounter = 0f;
            }
        }
        // Move towards current patrol target
        else
        {
            navMeshAgent.SetDestination(currentPatrolTarget.position);
        }
    }

    void SwitchPatrolTarget()
    {
        if (currentPatrolTarget == patrolPointA)
        {
            currentPatrolTarget = patrolPointB;
        }
        else
        {
            currentPatrolTarget = patrolPointA;
        }
    }
}