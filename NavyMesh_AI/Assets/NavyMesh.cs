using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavyMesh : MonoBehaviour
{
 public NavMeshAgent agent;

    [SerializeField] Transform _player;
    public LayerMask ground, player;
    public Vector3 destinationPoint;
    private bool destinationPointSet,alredyAttacked;
    public float walkPointRange,timeBeetweenAttacks;
    public GameObject mermi,mermiPos;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {



        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, player);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patoling();
           
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }
    void Patoling()
    {
        if (!destinationPointSet)
        {
            SearchWalkPoint();
        }
        if (destinationPointSet)
        {
            agent.SetDestination(destinationPoint);

        }
        Vector3 distanceToDestinationPoint=transform.position - destinationPoint;
        if(distanceToDestinationPoint.magnitude<1f)
        {
            destinationPointSet = false;
        }
    }
    void SearchWalkPoint()
    {
       float  randomX= Random.Range(-walkPointRange,walkPointRange);
       float  randomZ=Random.Range(-walkPointRange,walkPointRange);
        destinationPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(destinationPoint, -transform.up, 2.0f, ground))
        {
          
            destinationPointSet = true;
        }
    }
    void ChasePlayer()
    {
        agent.SetDestination(_player.position);
    }
    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(_player);
        if (!alredyAttacked)
        {
            Rigidbody rbSphere=Instantiate(mermi,mermiPos.transform.position,Quaternion.identity).GetComponent<Rigidbody>();
            rbSphere.AddForce(transform.forward * 25f, ForceMode.Impulse);
            rbSphere.AddForce(transform.up*7f,ForceMode.Impulse);
            alredyAttacked = true;
            Invoke(nameof(ResetAttack), timeBeetweenAttacks);
        }
    }
    void ResetAttack()
    {
     alredyAttacked=false;
    }


}
