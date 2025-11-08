using UnityEngine;

public class OwnerDetection : MonoBehaviour
{
    // 👇 This line creates the field you’ll see in the Inspector
    public GuardChase guard;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("👀 Owner spotted the player!");
            if (guard != null)
            {
                guard.StartChasing(other.transform);
            }
        }
    }
}
