using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScanManager : MonoBehaviour
{
    public AudioSource audioF;
    public AudioSource audioN;
    public GameObject foundScreen;
    public GameObject notFoundScreen;
    public static ScanManager instance;
    private float duration = 5.0f;
    public Slider slider;
    public Material scanningMaterial;
    public Material foundMaterial;
    public Material notFoundMaterial;
    private bool isScanning;

    void Start()
    {
        isScanning = false;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScanRoom(RoomScan roomScan)
    {
        if ((!isScanning))
        {
            StartCoroutine(flashScreenFound(roomScan));
        }
    }

    public IEnumerator flashScreenFound(RoomScan roomScan)
    {
        // Create a material array with one material (scanningMaterial) for the Renderer
        roomScan.roomRenderer.enabled = true;
        isScanning = true;
        float elapsed = 0f;

        // Ensure the slider starts at 0
        slider.value = 0f;

        // Loop over the duration
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            // Update slider value (normalized between 0 and 1)
            slider.value = Mathf.Clamp01(elapsed / duration);

            yield return null; // Wait until the next frame
        }

        // Determine the result and update the material accordingly
        if (roomScan.isInRoom)
        {
            roomScan.roomRenderer.material = foundMaterial;
            audioF.Play();
            foundScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(1.0f);
            foundScreen.SetActive(false);
        }
        else
        {
            roomScan.roomRenderer.material = notFoundMaterial;
            audioN.Play();
            notFoundScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(1.0f);
            notFoundScreen.SetActive(false);
        }

        isScanning = false;
        roomScan.roomRenderer.material = scanningMaterial;
        // Optionally clear the materials if necessary
        roomScan.roomRenderer.enabled = false;
    }
}
