using UnityEngine;
using UnityEngine.UI;

public class RoomScan : MonoBehaviour
{
    public bool isInRoom;
    public string roomName;
    public MeshRenderer roomRenderer;

    private void Start()
    {
        isInRoom = false;
        roomRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crim"))
        {
            isInRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Crim"))
        {
            isInRoom = false;
        }
    }
}
