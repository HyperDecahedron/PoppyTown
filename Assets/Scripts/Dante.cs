using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante : MonoBehaviour
{
    [SerializeField] private GameObject Ebutton;
    public string character; 

    [Header("Audio")]
    public AudioClip dante_sound;
    private AudioSource audioSource;
 
    private DialogManager dialogManager;
    private bool playerInRange = false;
    private PlayerController playerController;

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
        {
            Debug.Log("Character: DialogManager is null");
        }

        // Find Player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

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

    // STATES:

    // 0: waiting to accept farming
    // 1: accepted farming
    // 2: finished farming, waiting to accept fountain

    // 3: accepted fountain
    // 4: finished fountain, waiting to accept harvest

    // 5: accepted harvest
    // 6: finished harvest

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogManager != null)
            {
                Ebutton.SetActive(false);
                playerInRange = false;

                audioSource.PlayOneShot(dante_sound, 0.3f);

                if (character == "dante")
                {
                    if (playerController.state == 0)
                    {
                        // waiting to accept farming
                        dialogManager.SetTaskDialog("dante", "farming");
                    }
                    else if (playerController.state == 1)
                    {
                        // accepted farming task
                        dialogManager.SetTaskDialog("dante", "farming2");
                    }
                    else if (playerController.state == 2)
                    {
                        // finished farming task, waiting to accept fountain task
                        dialogManager.SetTaskDialog("default_state", "default_state");
                    }
                    else if (playerController.state == 3)
                    {
                        // accepted fountain task
                        dialogManager.SetTaskDialog("dante", "fountain");
                    }
                    else if (playerController.state == 4)
                    {
                        // finished fountain task, waiting to accept harvest
                        dialogManager.SetTaskDialog("default_state", "default_state");
                    }
                    else if (playerController.state == 5)
                    {
                        // accepted harvest task
                        dialogManager.SetTaskDialog("dante", "harvest");
                    }
                    else if (playerController.state == 6)
                    {
                        // finished all tasks
                        dialogManager.SetTaskDialog("default_state", "default_state");
                    }
                }

                else if (character == "polina")
                {
                    if (playerController.state == 0)
                    {
                        // waiting to accept farming
                        dialogManager.SetTaskDialog("default_state", "default_state");
                    }
                    else if (playerController.state == 1)
                    {
                        // accepted farming task
                        dialogManager.SetTaskDialog("polina", "farming");
                    }
                    else if (playerController.state == 2)
                    {
                        // finished farming task, waiting to accept fountain task
                        dialogManager.SetTaskDialog("polina", "fountain");
                    }
                    else if(playerController.state == 3)
                    {
                        // accepted fountain task
                        dialogManager.SetTaskDialog("polina", "fountain2");
                    }
                    else if(playerController.state == 4)
                    {
                        // finished fountain task, waiting to accept harvest
                        dialogManager.SetTaskDialog("default_state", "default_state");
                    }
                    else if (playerController.state == 5)
                    {
                        // accepted harvest task
                        dialogManager.SetTaskDialog("polina", "harvest");
                    }
                    else if (playerController.state == 6)
                    {
                        // finished all tasks
                        dialogManager.SetTaskDialog("default_state", "default_state");
                    }

                }

                else if (character == "belzy")
                {
                    if (playerController.state == 0)
                    {
                        // waiting to accept farming
                        dialogManager.SetTaskDialog("belzy", "default_belzy");
                    }
                    else if (playerController.state == 1)
                    {
                        // accepted farming task
                        dialogManager.SetTaskDialog("belzy", "farming");
                    }
                    else if (playerController.state == 2)
                    {
                        // finished farming task, waiting to accept fountain task
                        dialogManager.SetTaskDialog("belzy", "default_belzy");
                    }
                    else if (playerController.state == 3)
                    {
                        // accepted fountain task
                        dialogManager.SetTaskDialog("belzy", "fountain");
                    }
                    else if (playerController.state == 4)
                    {
                        // finished fountain task, waiting to accept harvest
                        dialogManager.SetTaskDialog("belzy", "harvest");
                    }
                    else if (playerController.state == 5)
                    {
                        // accepted harvest task
                        dialogManager.SetTaskDialog("belzy", "harvest2");
                    }
                    else if (playerController.state == 6)
                    {
                        // finished all tasks
                        dialogManager.SetTaskDialog("belzy", "default_belzy");
                    }

                }

            }
        }
    }


}
