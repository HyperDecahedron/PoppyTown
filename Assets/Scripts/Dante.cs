using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dante : MonoBehaviour
{
    [SerializeField] private GameObject Ebutton;

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

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogManager != null)
            {
                Ebutton.SetActive(false);
                playerInRange = false;

                if (playerController.state == 0)
                {
                    // set dialog for dante farming task
                    audioSource.PlayOneShot(dante_sound, 0.3f);
                    dialogManager.SetTaskDialog("dante", "farming");
                }
                else if(playerController.state == 1)
                {
                    audioSource.PlayOneShot(dante_sound, 0.3f);
                    dialogManager.SetTaskDialog("dante", "farming2");
                }
  
            }
        }
    }


}
