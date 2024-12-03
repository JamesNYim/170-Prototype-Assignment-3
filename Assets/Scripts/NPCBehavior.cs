using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NPCBehavior : MonoBehaviour, IPointerClickHandler {

    public NavMeshAgent agent;
    private List<Vector3> poiList;  
    private List<GameObject> allPois;
    public List<GameObject> runAwayPOIs;
    private bool isCriminal;
    private int currentPOIIndex = 0;
    public AudioSource criminalVisitedSFX;
    public GameObject timer;
    private int visitedPOIs = 0;
    private bool isRunningAway;

    float currentTime;

    protected virtual void initializeAIBehavior()
    {
        currentTime = Time.time;

        if (agent == null) {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
                return;
            }
        }

        allPois = new List<GameObject>(GameObject.FindGameObjectsWithTag("poitag"));
        poiList = new List<Vector3>();

        runAwayPOIs = new List<GameObject>(GameObject.FindGameObjectsWithTag("runAwayPOI"));

        if (allPois.Count == 0)
        {
            Debug.LogError("No POIs found with tag 'poitag'.");
            return;
        }

        if (isCriminal)
        {
            foreach (var poi in allPois)
            {
                poiList.Add(poi.transform.position);
            }
            ShuffleList(poiList);
        }
        else
        {
            int howManyPOIs = UnityEngine.Random.Range(1, allPois.Count);
            List<GameObject> tempPois = new List<GameObject>(allPois);
            for (int i = 0; i < howManyPOIs; i++)
            {
                int whichRemoved = UnityEngine.Random.Range(0, tempPois.Count);
                poiList.Add(tempPois[whichRemoved].transform.position);
                tempPois.RemoveAt(whichRemoved);
            }
            ShuffleList(poiList);
        }

        if (poiList.Count > 0)
        {
            MoveToNextPOI();
        }
        else
        {
            Debug.LogWarning("POI list is empty for " + gameObject.name);
        }
    }

    void Start() {
        initializeAIBehavior();
        isRunningAway = false;
    }
    void Update() {
            // If an agent reaches a POI
        if (agent != null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            if (isCriminal) {
                if (isRunningAway) {
                    isRunningAway = false;
                    agent.speed = 20f;
                    moveToCurrentPOI();
                    return;
                }
                else {
                    Debug.Log("criminal: " + agent.name + " has visited a POI: " + currentPOIIndex);
                    criminalVisitedSFX.Play(); 
                    visitedPOIs++;
                    // When the criminal visited all POI's start timer
                    if (visitedPOIs == allPois.Count) {
                        npcManagerScript.instance.callEndgameTimer();
                    }
                    // banner stuff
                    npcManagerScript.instance.StartCoroutine("typeString");
                }
                
            }
            MoveToNextPOI();
        }
    }

    void moveToCurrentPOI() {
        if (poiList == null || poiList.Count == 0 || agent == null) return;
        agent.SetDestination(poiList[currentPOIIndex]);
    }
    void MoveToNextPOI() {
        if (poiList == null || poiList.Count == 0 || agent == null) return;
        currentPOIIndex = (currentPOIIndex + 1) % poiList.Count;
        agent.SetDestination(poiList[currentPOIIndex]);
    }

    private void ShuffleList(List<Vector3> list) {
        for (int i = 0; i < list.Count; i++) {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            Vector3 temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void runAway() {
        npcManagerScript.instance.StartFlicker();
        isRunningAway = true;
        agent.speed = 100f;
        // Get a random index from the list
        int randomIndex = UnityEngine.Random.Range(0, runAwayPOIs.Count);
        // Retrieve the element at the random index
        Vector3 randomPOI = runAwayPOIs[randomIndex].transform.position;
        agent.SetDestination(randomPOI);
    }

    public void SetCriminalStatus(bool status) {
        isCriminal = status;
    }

    public bool getCriminalStatus() {
        return isCriminal;
    }

   public void SetMaterial(Material material) {
        GetComponent<Renderer>().material = material;
    }
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Are they a criminal? " + isCriminal);
    }
}
