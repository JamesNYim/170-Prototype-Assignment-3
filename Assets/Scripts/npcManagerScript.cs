using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class npcManagerScript : MonoBehaviour
{
    public Material civilianMaterial;  
    public Material markedMaterial; 
    public static npcManagerScript instance;

    List<GameObject> npcList;
    int NPC_COUNT = 10;

    public GameObject map;
    public bool isOn;

    void Start()
    {
        instance = this;
        npcList = new List<GameObject>();
        GameObject criminalPrefab = Resources.Load("Criminal") as GameObject;
        GameObject civilianPrefab = Resources.Load("Civilian") as GameObject;

        GameObject criminal = Instantiate(criminalPrefab);
        npcList.Add(criminal);

        for (int i = 0; i < NPC_COUNT - 1; i++)
        {
            GameObject civilian = Instantiate(civilianPrefab);
            npcList.Add(civilian);
        }
    }

    private void Update()
    {
        isOn = !isOn;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOn)
            {
                Cursor.lockState = CursorLockMode.None;
                //Time.timeScale = 0.0f;
            } else
            {
                Cursor.lockState = CursorLockMode.Locked;
                //Time.timeScale = 1.0f;
            }
            map.SetActive(isOn);    
        }
    }

    public IEnumerator bringMap()
    {

        yield return new WaitForSeconds(1);
        map.SetActive(!isOn);
    }
}


