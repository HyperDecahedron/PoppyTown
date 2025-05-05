using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string text;
    public GameObject Ebutton;
    public AudioClip button_sound;
    private AudioSource audioSource;

    private DialogManager dialogManager;
    private bool playerInRange = false;

    public bool isQuestItem = false;
    public string name;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        // Find the object tagged "Dialog" and get the DialogManager component
        GameObject dialogObject = GameObject.FindGameObjectWithTag("Dialog");
        if (dialogObject != null)
        {
            dialogManager = dialogObject.GetComponent<DialogManager>();
        }
        else
            Debug.Log("Character: DialogManager is null");

        Ebutton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Ebutton.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Ebutton.SetActive(false);
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogManager != null)
            {
                Ebutton.SetActive(false);
                audioSource.PlayOneShot(button_sound, 0.8f);
                dialogManager.SetDialog(text, null, null, !isQuestItem); // if it is quest item, set type to false
                playerInRange = false;

                if (isQuestItem)
                {
                    // Get player controller
                    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                    if (playerObject != null)
                    {
                        PlayerController playerController = playerObject.GetComponent<PlayerController>();
                        if (name == "watering_can")
                            playerController.has_watering_can = true;
                        else if (name == "seeds")
                            playerController.has_seeds = true;
                    }
                    else
                    {
                        Debug.Log("Character: PlayerController not found");
                    }

                    this.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}
