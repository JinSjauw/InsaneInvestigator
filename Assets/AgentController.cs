using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AgentController : MonoBehaviour
{
    [SerializeField] private ParticleSystem shockVFX;
    public event EventHandler<AgentController> OnAgentDeath;
    private NavMeshAgent agent;
    private List<Transform> patrolPointsList;
    
    private Vector3 currentDestination;
    private Vector3 lastDestination;
    private Vector3 currentPosition;

    private Animator animator;
    private Rigidbody agentBody;
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, currentDestination);
        if (distance < 1f)
        {
            lastDestination = currentDestination;
            currentDestination = patrolPointsList[Random.Range(0, patrolPointsList.Count)].position;
            agent.SetDestination(currentDestination);
        }
    }
    
    public void Die()
    {
        agent.isStopped = true;
        agent.enabled = false;
        animator.enabled = false;
        shockVFX.Play();
        agentBody.isKinematic = false;
        agentBody.useGravity = true;
        agentBody.AddTorque(transform.right * 10f);
        OnAgentDeath?.Invoke(this, this);
    }
    
    public void InitializeAgent(List<Transform> patrolPoints)
    {
        patrolPointsList = new List<Transform>();
        patrolPointsList = patrolPoints;
        currentDestination = patrolPointsList[Random.Range(0, patrolPointsList.Count)].position;
        
        shockVFX.Stop();
        lastDestination = Vector3.positiveInfinity;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(currentDestination);
        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", true);
        agentBody = GetComponent<Rigidbody>();
    }
}
