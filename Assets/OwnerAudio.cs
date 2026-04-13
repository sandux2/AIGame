using UnityEngine;

public class OwnerAudio : MonoBehaviour
{

    public AudioSource SireneSound; // sound effect for sirene when guard is chasing player
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySireneSound()
    {
        SireneSound.Play(); // play sirene sound effect
    }
}
