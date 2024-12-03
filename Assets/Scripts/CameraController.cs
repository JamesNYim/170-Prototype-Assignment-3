using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
public class CameraController : MonoBehaviour {
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;
    public float sensitivityZ = 100f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private float rotationZ = 0f;

    public int playerLives = 5;
    public int runAwayLives = 3;
    public TMP_Text strikesText;
    public TMP_Text statusText; 
    public Material markedMaterial;
    GameObject mainCamera;
    public LayerMask worldLayer;
    public CameraScript currentCamera = null;
    public GameObject livesEndScreen;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = GameObject.Find("Main Camera");
        SetCameraPos(currentCamera);
        

    }
    public void SetCameraPos(CameraScript newCam){
        currentCamera = newCam;
        mainCamera.transform.position = newCam.cameraPos;
        mainCamera.transform.LookAt(newCam.lookAtPos);
        Vector3 fromEuler = mainCamera.transform.localEulerAngles;
        rotationY = fromEuler.y;
        rotationX = fromEuler.x;
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivityY * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityX * Time.deltaTime;
        rotationY += mouseX;
        rotationX -= mouseY;
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

        // Handle object clicking
        if (Input.GetMouseButtonDown(0))
        {  // Left mouse button
            ClickObject();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            clickCamera();
        } else if(Input.GetKeyDown(KeyCode.S) && currentCamera && currentCamera.room)
        {
            ScanManager.instance.ScanRoom(currentCamera.room);
        }
    }

    // Method to cast a ray and detect objects on click
    private void ClickObject() {
        // Cast a ray from the camera to the mouse position
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if the ray hits any collider in the scene
        if (Physics.Raycast(ray, out hit)) {
            GameObject clickedObject = hit.collider.gameObject;
            Debug.Log("Clicked on object: " + clickedObject.name);

            // Optionally, you can trigger a specific action or method on the clicked object here
            // Example: if the object has a specific component, you can access it like this:
             NPCBehavior npcBehavior = hit.collider.GetComponent<NPCBehavior>();
            if (npcBehavior != null) {
                // Call isCriminal method to check if this NPC is a criminal
                if (npcBehavior.getCriminalStatus()) {
                    Debug.Log(hit.collider.gameObject.name + " is a criminal!");
                    npcBehavior.runAway();
                    runAwayLives--;
                    if (runAwayLives <= 0) {
                        EndGame(true);
                    }
                }
                else {
                    Debug.Log(hit.collider.gameObject.name + " is not a criminal.");
                    npcBehavior.SetMaterial(markedMaterial);
                    livesUpdate();
                    
                }
            }
        }
    }
    
    private void clickCamera()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~worldLayer))
        {
            GameObject clickedObject = hit.collider.gameObject;
            Debug.Log("Clicked on object: " + clickedObject.name);
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
            statusText.text = "Congratulations! You found a criminal. You won!";
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
        Debug.Log(playerLives);
        strikesText.text = "Lives Left: " + playerLives;
        if (playerLives <= 0) {
           npcManagerScript.instance.endGame(livesEndScreen); 
        }
    }
    

}
