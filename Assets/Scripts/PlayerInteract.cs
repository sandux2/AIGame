using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 5))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    hit.collider.GetComponent<DoorController>().ToggleDoor();
                }
            }
        }
    }
}