using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    
    public NavMeshAgent agent;
    public Transform pointOfInterest;
    // Update is called once per frame
    void Start() {
        pointOfInterest = GameObject.Find("POI").transform;
    }

    void Update()
    {
        agent.SetDestination(pointOfInterest.position);
        return;
    }
}
