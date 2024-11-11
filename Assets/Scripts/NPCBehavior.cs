using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine;

public class NPCBehavior : MonoBehaviour,  IPointerClickHandler {
    
    public NavMeshAgent agent;
    public Transform pointOfInterest;
    List<Vector3> poiList;
    List<GameObject> allPois;
    // Update is called once per frame
    float currentTime;
    void Start() {
        currentTime = Time.time;
        pointOfInterest = GameObject.Find("POI").transform;
        allPois = new List<GameObject>(GameObject.FindGameObjectsWithTag("poitag"));
        poiList = new List<Vector3>();
        int howManyPOIs = UnityEngine.Random.Range(1, allPois.Count);
        for(int i = 0; i < howManyPOIs; i++){ //How many pois should we assign  to each npc?
           int whichRemoved = UnityEngine.Random.Range(0, allPois.Count); //Remove the poi from the list to make sure we don't add the same poi twice to a npc's poilist
           poiList.Add(allPois[whichRemoved].transform.position);
           Debug.Log("what I added: " + poiList[poiList.Count - 1]);
           allPois.RemoveAt(whichRemoved);
        }
        agent.SetDestination(pointOfInterest.position);
    }

    void Update()
    {
        if((Time.time - currentTime) >= 10){
            currentTime = Time.time;
            Vector3 newPOI = poiList[(UnityEngine.Random.Range(0, poiList.Count))];
            Debug.Log("new poi: " + newPOI);
            agent.SetDestination(newPOI);
        }
        return;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("down");
    }
}
