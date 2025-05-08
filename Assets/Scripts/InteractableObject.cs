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

        // set inactive objects that the player already used
        if (isQuestItem)
        {
            // get player controller
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                PlayerController playerController = playerObject.GetComponent<PlayerController>();

                if (name == "watering_can" && playerController.has_watering_can)
                    this.transform.parent.gameObject.SetActive(false);
                else if (name == "seeds"&& playerController.has_seeds)
                    this.transform.parent.gameObject.SetActive(false);
                else if (name == "sponge" && playerController.has_cleaned1 && playerController.has_cleaned2 && playerController.has_cleaned3)
                    this.transform.parent.gameObject.SetActive(false);
                else if (name == "fountain1" && playerController.has_cleaned1)
                    this.transform.parent.gameObject.SetActive(false);
                else if (name == "fountain2" && playerController.has_cleaned2)
                    this.transform.parent.gameObject.SetActive(false);
                else if (name == "fountain3" && playerController.has_cleaned3)
                    this.transform.parent.gameObject.SetActive(false);
                else if(name == "diary" && playerController.has_diary)
                    this.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Character: PlayerController not found");
            }
        }
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
                playerInRange = false;

                if (name != "fountain1" && name != "fountain2" && name != "fountain3")
                    dialogManager.SetDialog(text, null, null, !isQuestItem); // if it is quest item, set type to false

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
                        else if (name == "sponge")
                            playerController.has_sponge = true;
                        else if (name == "fountain1" && playerController.has_sponge)
                        {
                            dialogManager.SetDialog(text, null, null, !isQuestItem);
                            playerController.has_cleaned1 = true;
                            this.transform.parent.gameObject.SetActive(false);
                        }   
                        else if (name == "fountain2" && playerController.has_sponge)
                        {
                            dialogManager.SetDialog(text, null, null, !isQuestItem);
                            playerController.has_cleaned2 = true;
                            this.transform.parent.gameObject.SetActive(false);
                        }    
                        else if (name == "fountain3" && playerController.has_sponge)
                        {
                            dialogManager.SetDialog(text, null, null, !isQuestItem);
                            playerController.has_cleaned3 = true;
                            this.transform.parent.gameObject.SetActive(false);
                        }  
                        else if(name == "poppy")
                        {
                            playerController.poppies++;
                        }
                        else if(name == "diary")
                        {
                            playerController.has_diary = true;
                        }
                    }
                    else
                    {
                        Debug.Log("Character: PlayerController not found");
                    }

                    if(name!= "fountain1" && name != "fountain2" && name != "fountain3")
                        this.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}
