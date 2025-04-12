using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToScene : MonoBehaviour
{
    public string changeToScene;
    public SceneChanger sceneChanger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.prevScene = currentScene;
                Debug.Log("Updated prevScene to: " + currentScene);
            }
            else
                Debug.Log("PlayerController component not found on the Player!");

            if (sceneChanger != null)
                sceneChanger.triggerCollision(changeToScene);
            else
                Debug.Log("SceneChanger reference is missing!");
        }
    }

}
