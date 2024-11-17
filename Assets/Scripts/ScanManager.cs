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
        isScanning = true;
        float elapsed = 0f;

        // Ensure the slider starts at 0
        slider.value = 0f;

        // Loop over the duration
        while (elapsed < duration)
        {
            // Increment elapsed time
            elapsed += Time.unscaledDeltaTime;

            // Update slider value (normalized between 0 and 1)
            slider.value = Mathf.Clamp01(elapsed / duration);

            yield return null; // Wait until the next frame
        }

        if(roomScan.isInRoom)
        {
            audioF.Play();
            foundScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            foundScreen.SetActive(false);
            isScanning = false;
        } else
        {
            audioN.Play();
            notFoundScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(.5f);
            notFoundScreen.SetActive(false);
            isScanning = false;
        }
    }
}
