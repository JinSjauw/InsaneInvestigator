using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField] private List<Transform> waypointsList;
    // Start is called before the first frame update
    [SerializeField] private Transform agentPrefab;
    [SerializeField] private List<Transform> spawnPoints;

    [SerializeField] private float spawnRadius;
    [SerializeField] private int maxActiveAgents;
    [SerializeField] private float spawnCheckInterval;
    [SerializeField] private float spawnDelay;
    private float spawnTimer = 0;
    
    private List<AgentController> m_ActiveAgentList;
    
    #region Unity Functions
    private void Awake()
    {
        m_ActiveAgentList = new List<AgentController>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnCheckInterval)
        {
            StartCoroutine(Spawn());
            spawnTimer = 0;
        }
    }
    #endregion

    #region Private Functions
    private IEnumerator Spawn()
    {
        if (m_ActiveAgentList.Count > maxActiveAgents) yield return null;

        int spawnAmount = maxActiveAgents - m_ActiveAgentList.Count;

        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            Vector2 pointInCircle = Random.insideUnitCircle * spawnRadius;

            randomSpawnPoint += new Vector3(pointInCircle.x, 0, pointInCircle.y);
            
            AgentController spawnedAgent = Instantiate(agentPrefab, randomSpawnPoint, Quaternion.identity).GetComponent<AgentController>();
            spawnedAgent.transform.SetParent(transform);
            spawnedAgent.SetPatrolPoints(waypointsList);
            spawnedAgent.OnAgentDeath += OnAgentDeath;
            
            m_ActiveAgentList.Add(spawnedAgent);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void OnAgentDeath(object sender, AgentController e)
    {
        if (m_ActiveAgentList.Contains(e))
        {
            m_ActiveAgentList.Remove(e);
            e.OnAgentDeath -= OnAgentDeath;
        }   
    }

    #endregion
}
