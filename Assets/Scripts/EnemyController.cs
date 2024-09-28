using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Stalk,
        Chase
    }

    public float sightRange = 10f;
    public float stalkDuration = 30f;
    public float chaseDuration = 15f;
    public float patrolWaitTime = 3f;
    public float lightCheckInterval = 0.5f;
    public LayerMask playerLayer;
    public LayerMask lightLayer;
    public LayerMask doorLayer;
    public bool drawDebugRays = true;

    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.Patrol;
    private GameObject player;
    private List<Vector3> patrolPoints;
    private int currentPatrolIndex = 0;
    private float stateTimer = 0f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        GeneratePatrolPoints();

        // Ensure the agent is on the NavMesh
        if (PlaceAgentOnNavMesh())
        {
            StartCoroutine(StateMachine());
        }
        else
        {
            Debug.LogError("Failed to place the agent on the NavMesh. Disabling EnemyController.");
            enabled = false;
        }
    }

    private bool PlaceAgentOnNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            return true;
        }
        return false;
    }

    private void GeneratePatrolPoints()
    {
        // TODO: Implement a method to generate patrol points in the dungeon
        // For now, we'll use a simple example with fixed points
        patrolPoints = new List<Vector3>
        {
            new Vector3(10, 0, 10),
            new Vector3(-10, 0, 10),
            new Vector3(-10, 0, -10),
            new Vector3(10, 0, -10)
        };
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            //Debug.Log(currentState);
            switch (currentState)
            {
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Stalk:
                    Stalk();
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
            }

            yield return null;
        }
    }


    private void Patrol()
    {
        if (CanSeePlayer())
        {
            TransitionToState(EnemyState.Stalk);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPatrolIndex]);
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        yield return new WaitForSeconds(patrolWaitTime);
    }

    private void Stalk()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= stalkDuration)
        {
            TransitionToState(EnemyState.Chase);
            return;
        }

        if (!CanSeePlayer())
        {
            TransitionToState(EnemyState.Patrol);
            return;
        }

        if (!IsInLight())
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
            StartCoroutine(WaitForLightToTurnOff());
        }
    }

    private IEnumerator WaitForLightToTurnOff()
    {
        while (IsInLight())
        {
            yield return new WaitForSeconds(lightCheckInterval);
        }
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
    }

    private void Chase()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= chaseDuration)
        {
            TransitionToState(EnemyState.Patrol);
            return;
        }

        if (IsDoorClosedBetweenEnemyAndPlayer())
        {
            TransitionToState(EnemyState.Patrol);
            return;
        }

        agent.SetDestination(player.transform.position);

        if (IsInLight())
        {
            // TODO: Implement disappearing behavior
            Debug.Log("Enemy disappeared in light!");
            TransitionToState(EnemyState.Patrol);
        }
    }

    private void TransitionToState(EnemyState newState)
    {
        currentState = newState;
        stateTimer = 0f;
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }

        switch (currentState)
            {
                case EnemyState.Patrol:
                    // code for entering patrol
                    animator.Play("PatrolAnimation");
                    break;
                case EnemyState.Stalk:
                    // code for stalking
                    animator.Play("StalkAnimation");
                    break;
                case EnemyState.Chase:
                    // code for chasing
                    animator.Play("ChaseAnimation");
                    break;
            }

    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= sightRange)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange, playerLayer))
            {
                if (drawDebugRays)
                {
                    Debug.DrawRay(transform.position, directionToPlayer * sightRange, Color.red, 0.1f);
                }
                var hitPlayer = hit.collider.gameObject == player;
                if (hitPlayer){
                    Debug.Log("Player sighted");
                }
                return hitPlayer;
            }
        }
        return false;
    }

    private bool IsInLight()
    {
        Collider[] lightColliders = Physics.OverlapSphere(transform.position, 0.1f, lightLayer);
        return lightColliders.Length > 0;
    }

    private bool IsDoorClosedBetweenEnemyAndPlayer()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange, doorLayer))
        {
            if (drawDebugRays)
            {
                Debug.DrawRay(transform.position, directionToPlayer * hit.distance, Color.blue, 0.1f);
            }
            // TODO: Implement a method to check if the door is closed
            // For now, we'll assume any door hit means it's closed
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}