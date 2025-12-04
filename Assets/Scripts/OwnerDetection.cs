using UnityEngine;

public class OwnerDetection : MonoBehaviour
{

    public GuardChase guard; // reference to GuardChase script

    private void OnTriggerEnter(Collider other) // detect player entering owner's area
    {
        if (other.CompareTag("Player")) // check for player tag
        {
            Debug.Log("Owner spotted the player!");
            if (guard != null)
            {
                guard.StartChasing(other.transform); // tell guard to chase player
            }
        }
    }
}
