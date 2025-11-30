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
        // Initialize NavMeshAgent
        navMeshAgent = GetComponent<NavMeshAgent>();

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
        if (currentPatrolTarget == null)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, currentPatrolTarget.position);

        // If reached patrol point, wait and switch target
        if (distanceToTarget < 1f)
        {
            patrolWaitCounter += Time.deltaTime;

            if (patrolWaitCounter >= patrolWaitTime)
            {
                SwitchPatrolTarget();
                patrolWaitCounter = 0f;
            }
        }
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