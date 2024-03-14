using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navMeshAgent : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private NavMeshAgent meshAgent;

    // Start is called before the first frame update
    void Start()
    {
        meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        meshAgent.SetDestination(target.transform.position);
    }

}
