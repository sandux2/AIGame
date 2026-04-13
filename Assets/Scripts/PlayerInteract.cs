using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public GameObject ItemParent;
    public List<Transform> Items = new List<Transform>(); // list to store items in the scene
    public int ItemsAmount;
    public int ItemsCollected;
    public GameObject selectedItems;
    GameObject grabItem;
    public Text itemCounterText; // UI text to display item count
    public GameObject interactPrompt; // UI prompt for interaction
    public GameObject winScreen; // UI screen for winning the game
    public AudioSource collectSound; // sound effect for collecting items
    void Start()
    {

        ItemsCollected = 0;

        foreach(Transform item in ItemParent.transform) // get all items from parent object
        {
            Items.Add(item);

        }
        ItemsAmount = Items.Count; // store total number of items
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E)) // interact key
        {
            Ray ray = new Ray(transform.position, transform.forward); // ray forward from player
            if (Physics.Raycast(ray, out RaycastHit hit, 5)) 
            {
                if (hit.collider.CompareTag("Door")) // check for door tag
                {
                    hit.collider.GetComponent<DoorController>().ToggleDoor(); // toggle door
                }
            }
        }

        if(selectedItems != null)
        {
             if(Input.GetKeyDown(KeyCode.H)) // collect item key
            {
            
            ItemsCollected++;
            grabItem = selectedItems; // store reference to item being collected
            selectedItems.GetComponent<Collider>().enabled = false; // disable item collider
            selectedItems = null; // reset selected item
            Debug.Log("Collected an item! Total: " + ItemsCollected + "/" + ItemsAmount);
            itemCounterText.text = "Items Collected " + ItemsCollected + "/" + ItemsAmount; // update UI text
            GetComponent<Animator>().SetTrigger("Pick"); // play collect animation
            interactPrompt.SetActive(false); // hide interaction prompt
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) // check for item tag
        {
            Debug.Log("Press H to collect item)");
            selectedItems = other.gameObject; // set selected item
            interactPrompt.SetActive(true); // show interaction prompt

            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item")) // check for item tag
        {
            selectedItems = null; // reset selected item
            interactPrompt.SetActive(false); // hide interaction prompt
        }
           
        
    }

    public void CollectItem()
    {
        grabItem.SetActive(false); // hide item
        grabItem = null; // reset selected item
        GetComponent<Animator>().ResetTrigger("Pick"); // reset animation trigger
        collectSound.Play(); // play collect sound effect
        if (ItemsCollected >= ItemsAmount) // check if all items collected
        {
            Debug.Log("All items collected! You win!");
            winScreen.SetActive(true); // show win screen
            Time.timeScale = 0; // pause the game
        }
    }
}