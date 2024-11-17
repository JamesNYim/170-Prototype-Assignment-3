using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
public class CameraController : MonoBehaviour {
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;
    public float sensitivityZ = 100f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private float rotationZ = 0f;

    public int playerLives = 5;
    public TMP_Text statusText; 
    public Material markedMaterial;
    private int cameraIndex;
    GameObject mainCamera;
    private List<Vector3> cameraList= new List<Vector3>{
        new Vector3(-402.7f, 100.0f, -347.6f), 
        new Vector3(-395.2f, 100.0f, 401.7f), 
        new Vector3(-33.0f, 100.0f, -353.59f), 
        new Vector3(51.0f, 100.0f, 330.0f), 
        new Vector3(18.0f, 100.0f, 76.0f), 
        new Vector3(419.58f, 100.0f, -348.83f), 
        new Vector3(419.58f, 100.0f, -42.1f), 
        new Vector3(334.0f, 100.0f, 216.0f)
    };

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = GameObject.Find("Main Camera");
        mainCamera.transform.position = cameraList[0];
        mainCamera.transform.LookAt(new Vector3(0, 0, 0));
        cameraIndex = 0;
    }
    public void SetCameraPos(int which){
        mainCamera.transform.position = cameraList[which - 1];
    }

    public void nextCamera()
    {
        cameraIndex++;
        if (cameraIndex >= cameraList.Count)
        {
            cameraIndex = 0;
        }
        mainCamera.transform.position = cameraList[cameraIndex];
        mainCamera.transform.LookAt(npcManagerScript.instance.gameObject.transform.position);
    }

    public void previousCamera()
    {
        cameraIndex--;
        if(cameraIndex < 0)
        {
            cameraIndex = cameraList.Count - 1;
        }
        mainCamera.transform.position = cameraList[cameraIndex];
        mainCamera.transform.LookAt(npcManagerScript.instance.gameObject.transform.position);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            previousCamera();
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextCamera();
        } else
        {
            if (!npcManagerScript.instance.isOn)
            {
                // Handle camera rotation
                float mouseX = Input.GetAxis("Mouse X") * sensitivityY * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * sensitivityX * Time.deltaTime;

                if (Input.GetMouseButton(1))
                {
                    float mouseZ = Input.GetAxis("Mouse X") * sensitivityZ * Time.deltaTime;
                    rotationZ += mouseZ;
                }

                rotationY += mouseX;
                rotationX -= mouseY;
                transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

                // Handle object clicking
                if (Input.GetMouseButtonDown(0))
                {  // Left mouse button
                    ClickObject();
                }
            }
        }
    }

    // Method to cast a ray and detect objects on click
    private void ClickObject() {
        // Cast a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
                    EndGame(true);
                }
                else {
                    Debug.Log(hit.collider.gameObject.name + " is not a criminal.");
                    npcBehavior.SetMaterial(markedMaterial);
                    
                }
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

    

}
