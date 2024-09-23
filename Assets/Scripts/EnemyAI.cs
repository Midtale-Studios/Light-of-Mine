using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public float speed = 5.0f;
    [SerializeField] public float detectionRange = 10.0f;
    [SerializeField] private Transform target;
    [SerializeField] private float wanderRadius = 5.0f;
    [SerializeField] private float wanderTimer = 3.0f;
    [SerializeField] private float rotationSpeed = 30.0f; // Rotation speed in degrees per second

    private float timer;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        timer = wanderTimer;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Wander();
            // Perform raycasting to find the player
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * detectionRange;
            Debug.DrawRay(transform.position, forward, Color.red);

            if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Player sighted, chasing!");
                    target = hit.transform;
                }
            }
        }
        else
        {
            // Move towards the player using NavMeshAgent
            agent.SetDestination(target.position);
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        // Avoid obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.0f))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                transform.Rotate(0, Random.Range(90, 270), 0);
            }
        }

        // Slowly rotate on the y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;
        randDirection.y = origin.y; // Keep the same height

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}