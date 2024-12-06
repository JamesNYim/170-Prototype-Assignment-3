using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections;
using Unity.VisualScripting;
public class CameraController : MonoBehaviour {
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;
    public float sensitivityZ = 100f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private float rotationZ = 0f;

    public int playerLives = 30;
    public int runAwayLives = 3;
    public TMP_Text statusText;
    public Material markedMaterial;
    public LayerMask worldLayer;
    public CameraScript currentCamera;
    public GameObject livesEndScreen;
    public float lastClickedTime = 0;
    Sprite health2;
    Sprite health1;
    public TextMeshProUGUI roomText;
    public Slider crosshair;

    // Clicking statuses
    public TMP_Text accusationsText;
    public TMP_Text criminalLivesText;
    //public GameObject healthBar;
    //public GameObject enemyCharge;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        SetCameraPos(currentCamera);
        criminalLivesText.text = "Criminal Lives: " + runAwayLives;
        accusationsText.text = "Accusations Left: " + playerLives;
        //health2 = Resources.Load<Sprite>("2Charge");
        //health1 = Resources.Load<Sprite>("1Charge");
        lastClickedTime = Time.time;
    }

    public void SetCameraPos(CameraScript newCam){
        currentCamera = newCam;
        Camera.main.transform.position = newCam.cameraPos;
        Camera.main.transform.LookAt(newCam.lookAtPos);
        Vector3 fromEuler = Camera.main.transform.localEulerAngles;
        rotationY = fromEuler.y;
        rotationX = fromEuler.x;
        roomText.text = newCam.room.roomName;
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityY * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityX * Time.deltaTime;
        rotationY += mouseX;
        rotationX -= mouseY;
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

        float timeDif = Time.time - lastClickedTime;
        timeDif = Mathf.Min(3.0f, timeDif);
        crosshair.value = timeDif / 3.0f;
        // Handle object clicking
        if (Input.GetMouseButtonDown(0))
        {  // Left mouse button
            ClickObject(timeDif);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            clickCamera();
            statusText.text = "Switching camera to new room";
        } else if(Input.GetKeyDown(KeyCode.S) && currentCamera && currentCamera.room)
        {
            statusText.text = "Scanning the room for criminal";
            ScanManager.instance.ScanRoom(currentCamera.room);
        }
    }

    // Method to cast a ray and detect objects on click
    private void ClickObject(float timeDif) {
        // Cast a ray from the camera to the mouse position
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        statusText.text = "Scanner on cooldown";
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 500.0f, Color.red, 50.0f); // <--- Debug
        RaycastHit hit;

        // Check if the ray hits any collider in the scene
        if (Physics.Raycast(ray, out hit)) {
            GameObject clickedObject = hit.collider.gameObject;
            Debug.Log("Clicked on object: " + clickedObject.name);

            // Optionally, you can trigger a specific action or method on the clicked object here
            // Example: if the object has a specific component, you can access it like this:
            NPCBehavior npcBehavior = hit.collider.GetComponent<NPCBehavior>();
            if (npcBehavior != null && (timeDif >= 3.0f)) {
                lastClickedTime = Time.time;
                // Call isCriminal method to check if this NPC is a criminal
                if (npcBehavior.getCriminalStatus()) {
                    statusText.text = "You found the criminal!";
                    npcBehavior.runAway();
                    runAwayLives--;
                    if(npcManagerScript.instance.timer.activeSelf)
                    {
                        runAwayLives = 0;
                    }
                    enemyLivesUpdate();
                    if (runAwayLives == 0) {
                        EndGame(true);
                    }
                }
                else {
                    statusText.text = "Civilian marked innocent";
                    npcBehavior.SetMaterial(markedMaterial);
                    livesUpdate();
                }
            }
        }
    }
    
    private void clickCamera()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 500.0f, Color.red, 50.0f); // <--- Debug
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, worldLayer))
        {
            GameObject clickedObject = hit.collider.gameObject;
            CameraScript camera = hit.collider.GetComponent<CameraScript>();
            if (camera != null)
            {
                SetCameraPos(camera);
            }
        }
    }

    void EndGame(bool success)
    {
        if (success)
        {
            statusText.text = "Congratulations! You found the criminal. You won!";
        }
        else
        {
            statusText.text = "Game Over! No lives left.";
        }

        // Disable further interaction
        this.enabled = false;
    }

    private void livesUpdate() {
        playerLives--;
        accusationsText.text = "Accusations Left: " + playerLives;
        if (playerLives <= 0) {
           EndGame(false);
           npcManagerScript.instance.endGame(livesEndScreen); 
        }
        /*
        if(playerLives == 3){
            healthBar.GetComponent<Image>().sprite = health2;
        }
        if(playerLives == 1){
            healthBar.GetComponent<Image>().sprite = health1;
        }
        */
        
    }

    private void enemyLivesUpdate()
    {
        criminalLivesText.text = "Criminal Lives: " + runAwayLives;
        /*
        if (runAwayLives == 2)
        {
            enemyCharge.GetComponent<Image>().sprite = health2;
        }
        if (playerLives == 1)
        {
            enemyCharge.GetComponent<Image>().sprite = health1;
        }
        */
    }


}
