using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine;

public class NPCBehavior : MonoBehaviour,  IPointerClickHandler {
    
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

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("down");
    }
}
