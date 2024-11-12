using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NPCBehavior : MonoBehaviour, IPointerClickHandler {

    public NavMeshAgent agent;
    public Transform pointOfInterest;
    private List<Vector3> poiList;  
    private List<GameObject> allPois;
    private bool isCriminal;
    private int currentPOIIndex = 0;

    float currentTime;

    protected virtual void Start() {
        currentTime = Time.time;

        // Ensure agent is assigned
        if (agent == null) {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null) {
                Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
                return;
            }
        }

        allPois = new List<GameObject>(GameObject.FindGameObjectsWithTag("poitag"));
        poiList = new List<Vector3>();

        if (allPois.Count == 0) {
            Debug.LogError("No POIs found with tag 'poitag'.");
            return;
        }

        // Assign POIs based on whether the NPC is a criminal
        if (isCriminal) {
            foreach (var poi in allPois) {
                poiList.Add(poi.transform.position);
            }
        } else {
            int howManyPOIs = UnityEngine.Random.Range(1, allPois.Count);
            List<GameObject> tempPois = new List<GameObject>(allPois);
            //Debug.Log("how many pois: " + howManyPOIs);
            for (int i = 0; i < howManyPOIs; i++) {
                int whichRemoved = UnityEngine.Random.Range(0, tempPois.Count);
                poiList.Add(tempPois[whichRemoved].transform.position);
                tempPois.RemoveAt(whichRemoved);
            }
        }

        if (poiList.Count > 0) {
            MoveToNextPOI();
        } else {
            Debug.LogWarning("POI list is empty for " + gameObject.name);
        }
    }

    void MoveToNextPOI() {
        if (poiList == null || poiList.Count == 0 || agent == null) return;
        
        agent.SetDestination(poiList[currentPOIIndex]);
    }

    void Update() {
        if (agent != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            currentPOIIndex = (currentPOIIndex + 1) % poiList.Count;
            if (isCriminal) {
                Debug.Log("criminal: " + agent.name + " has visited a POI: " + currentPOIIndex);
            }
            MoveToNextPOI();
        }
    }

    public void SetCriminalStatus(bool status) {
        isCriminal = status;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Are they a criminal? " + isCriminal);
    }
}
