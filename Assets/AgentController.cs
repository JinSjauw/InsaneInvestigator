using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public event EventHandler<AgentController> OnAgentDeath;
    [SerializeField] private ParticleSystem shockVFX;
    private NavMeshAgent agent;
    private List<Transform> patrolPointsList;

    // Start is called before the first frame update
    void Start()
    {
        shockVFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        OnAgentDeath?.Invoke(this, this);
    }
    
    public void SetPatrolPoints(List<Transform> patrolPoints)
    {
        patrolPointsList = patrolPoints;
    }
}
