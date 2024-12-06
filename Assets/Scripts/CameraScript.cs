using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public Vector3 cameraPos;
    public Vector3 lookAtPos;
    public RoomScan room;
    public Vector2 UIPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.cameraPos = transform.GetChild(1).position;
        this.lookAtPos = transform.GetChild(2).position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
