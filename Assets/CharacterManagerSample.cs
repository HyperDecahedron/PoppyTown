using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManagerSample : MonoBehaviour
{
    [SerializeField] private GameObject Ebutton;
    private DialogManager dialogManager;
    private bool playerInRange = false;

    private void Start()
    {
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

                dialogManager.SetDialog(
                    "Welcome to our village, traveler!",
                    "Thank you.",
                    "Who are you?"
                );

                playerInRange = false;
            }
        }
    }
}

