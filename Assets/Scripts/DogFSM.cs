using UnityEngine;

using UnityEngine.AI;

public enum DogState { Stay, Sit, Walk, ChaseEnemy, Attack, ChaseDestination }

public class DogFSM : MonoBehaviour
{
    public DogState CurrentState { get; set; }

    public Sight sightSensor;

    public float enemyDistance = 1;

    NavMeshAgent agent;

    Animator animator;

    [SerializeField] GameObject pizza;

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInParent<Animator>();
    }

    void Start()
    {
        NavMeshHit closestHit;
 
        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
            gameObject.transform.position = closestHit.position;
        else
            Debug.LogError("Could not find position on NavMesh!");
    }

    void Update()
    {
        if (CurrentState == DogState.Stay)
            Stay();
        else if (CurrentState == DogState.Walk)
            Walk();
        else if (CurrentState == DogState.ChaseEnemy)
            ChaseEnemy();
        else if (CurrentState == DogState.Attack)
            Attack();
        else if (CurrentState == DogState.ChaseDestination)
            ChaseDestination();

        if (sightSensor.detectedObject != null)
        {
            CurrentState = DogState.ChaseEnemy;

            float distanceToEnemy = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);
            if (distanceToEnemy <= enemyDistance)
                CurrentState = DogState.Attack;
            else
                CurrentState = DogState.ChaseEnemy;
        }
    }

    void Stay()
    {
        agent.isStopped = true;
    }

    void LookTo(Vector3 targetPosition)
    {
        Vector3 directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
        directionToPosition.y = 0;
        transform.parent.forward = directionToPosition;
    }

    void Walk()
    {
    }

    void ChaseEnemy()
    {
        if (sightSensor.detectedObject != null)
        {
            agent.isStopped = false;
            agent.SetDestination(sightSensor.detectedObject.transform.position);

            float distanceToEnemy = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);
            if (distanceToEnemy <= enemyDistance)
            {
                agent.isStopped = true;
                CurrentState = DogState.Attack;
            }
        }
    }

    Vector3 dest;
    void ChaseDestination(Vector3? destination = null)
    {
        if (destination != null)
        {
            dest = (Vector3)destination;
        }

        if (dest != null)
        {
            agent.isStopped = false;
            agent.SetDestination(dest);

            animator.SetFloat("Speed_f", 1f);

            float distanceToDest = Vector3.Distance(transform.position, dest);
            if (distanceToDest <= enemyDistance / 3)
            {
                agent.isStopped = true;
                animator.SetFloat("Speed_f", 0f);
                CurrentState = DogState.Stay;
            }
            // else
            //     Debug.Log(distanceToDest + "    " + enemyDistance);
        }
    }

    void Attack()
    {
        if (sightSensor.detectedObject != null)
        {
            animator.SetFloat("Speed_f", 0f);
            LookTo(sightSensor.detectedObject.transform.position);

            GameObject pizzaObject = Instantiate(pizza, transform.position + (sightSensor.detectedObject.transform.position - transform.position) / 3, Quaternion.identity);
            Destroy(pizzaObject, .1f);
            Destroy(sightSensor.detectedObject.gameObject, 1.5f);

            // EnemyManager.camera2.gameObject.SetActive(false);
            GameObject cam = GameObject.FindGameObjectWithTag("Info Camera");
            if (cam)
                cam.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyDistance);
    }
}
