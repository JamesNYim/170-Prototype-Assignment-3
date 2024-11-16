using UnityEngine;
using UnityEngine.UI;

public class RoomScan : MonoBehaviour
{
    public bool isInRoom;

    private void Start()
    {
        isInRoom = false;
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
