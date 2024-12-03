using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

public class npcManagerScript : MonoBehaviour
{
    public Material civilianMaterial;  
    public Material markedMaterial; 
    public static npcManagerScript instance;

    List<GameObject> npcList;
    int NPC_COUNT = 10;

    public GameObject timer;

    public GameObject banner;
    public TextMeshProUGUI textMeshProUGUI;
    private string message = "CRIMINAL HAS VISITED ANOTHER POINT OF INTEREST";
    private int stringLen;
    private int currentLen;
    public float flickerDuration = 0.75f;
    public float flickerInterval = 0.1f;
    public float blackoutDuration = 5f;
    public GameObject blackPanel;


    public AudioSource au;

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
        stringLen = message.Length;
    }

    private IEnumerator typeString()
    {
        banner.SetActive(true);
        currentLen = 0;
        textMeshProUGUI.text = "";
        while (currentLen <= stringLen)
        {
            textMeshProUGUI.text = message.Substring(0, currentLen);
            currentLen++;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(5f);

        banner.SetActive(false);
    }

    public void callEndgameTimer()
    {
        timer.SetActive(true);
        return;
    }

    private void Update()
    {

    }

    public void endGame(GameObject endScene)
    {
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        endScene.SetActive(true);
        au.Stop();
    }


    public void restartScene()
    {
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("SampleScene");
    }

    public void StartFlicker()
    {
        StartCoroutine(FlickerCoroutine());
    }

    private IEnumerator FlickerCoroutine()
    {
        float elapsed = 0f;

        // Flicker the screen on and off
        while (elapsed < flickerDuration)
        {
            blackPanel.SetActive(!blackPanel.activeSelf); // Toggle panel visibility
            yield return new WaitForSeconds(flickerInterval); // Wait for the interval
            elapsed += flickerInterval;
        }

        // Ensure the screen is fully black after flickering
        blackPanel.SetActive(true);

        // Wait for the blackout duration
        yield return new WaitForSeconds(blackoutDuration);

        // Restore the screen to normal
        blackPanel.SetActive(false);
    }
}


