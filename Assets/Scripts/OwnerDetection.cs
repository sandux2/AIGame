using UnityEngine;

public class OwnerDetection : MonoBehaviour
{
   
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
