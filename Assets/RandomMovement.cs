using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("RandomDestination", 0, 3); // 일정 간격으로 새로운 목적지 설정
        animator = GetComponent<Animator>();
    }

    void RandomDestination()
    {
        if (!isMoving)
        {
            Vector3 randomPoint = RandomNavSphere(transform.position, 200, 1);
            agent.SetDestination(randomPoint);
            isMoving = true;
        }
        else
        {
            animator.Play("Run Forward");
        }
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isMoving = false;
        }
    }

    // 무작위로 워크러블 지역 선택
    Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
        return navHit.position;
    }
}
