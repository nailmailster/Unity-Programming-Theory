using System.Collections;
using UnityEngine;

using UnityEngine.AI;

public enum CowState { Stay, Eat, Walk, ChaseFood, Eating, ChaseDestination }

public class CowFSM : MonoBehaviour
{
    public CowState CurrentState { get; set; }

    public Sight sightSensor;

    public float eatDistance = 3;

    NavMeshAgent agent;

    Animator animator;

    private void Awake()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        NavMeshHit closestHit;
 
        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
            gameObject.transform.position = closestHit.position;
        else
            Debug.LogError("Could not find position on NavMesh!");
    }
    
    void Update()
    {
        if (CurrentState == CowState.Stay)
            Stay();
        else if (CurrentState == CowState.Eat)
            Eat();
        else if (CurrentState == CowState.Walk)
            Walk();
        else if (CurrentState == CowState.ChaseFood)
            ChaseFood();
        else if (CurrentState == CowState.ChaseDestination)
            ChaseDestination();
    }

    void Stay()
    {
        agent.isStopped = true;

        animator.SetBool("Eat_b", false);
        animator.SetFloat("Speed_f", 0);

        if (sightSensor.detectedObject != null)
        {
            CurrentState = CowState.ChaseFood;

            float distanceToFood = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);
            if (distanceToFood <= eatDistance)
                CurrentState = CowState.Eat;
            else
                CurrentState = CowState.ChaseFood;
        }
    }

    void Eat()
    {
        LookTo(sightSensor.detectedObject.transform.position);

        CurrentState = CowState.Eating;
        animator.SetBool("Eat_b", true);
        StartCoroutine(Eating());
    }

    IEnumerator Eating()
    {
        yield return new WaitForSeconds(2);
        if (sightSensor.detectedObject != null)
        {
            Destroy(sightSensor.detectedObject.gameObject);
            sightSensor.detectedObject = null;
            // scale in
            // transform.parent.localScale *= 1.01f;
        }
        CurrentState = CowState.Stay;
        agent.isStopped = false;
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

    void ChaseFood()
    {
        if (sightSensor.detectedObject != null)
        {
            agent.isStopped = false;
            agent.SetDestination(sightSensor.detectedObject.transform.position);

            animator.SetBool("Eat_b", false);
            animator.SetFloat("Speed_f", 0.5f);

            float distanceToFood = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);
            if (distanceToFood <= eatDistance)
            {
                agent.isStopped = true;
                CurrentState = CowState.Stay;
            }
            // else
            //     Debug.Log(distanceToFood + "    " + eatDistance);
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

            animator.SetBool("Eat_b", false);
            animator.SetFloat("Speed_f", 0.5f);

            float distanceToFood = Vector3.Distance(transform.position, dest);
            if (distanceToFood <= eatDistance)
            {
                agent.isStopped = true;
                CurrentState = CowState.Stay;
            }
            // else
            //     Debug.Log(distanceToFood + "    " + eatDistance);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, eatDistance);
    }
}
