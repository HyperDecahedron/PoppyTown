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

                if (sceneChanger != null)
                {
                    if(changeToScene == "MCHouseScene" && playerController.state >= 6)
                    {
                        // first, stop time
                        GameObject time = GameObject.FindGameObjectWithTag("Time");
                        if (time != null)
                        {
                            GameTimeManager timeManager = time.GetComponent<GameTimeManager>();
                            timeManager.stopTime = true;
                            time.SetActive(false);
                        }
                        else
                        {
                            Debug.Log("Time not found when changing to Final Scene.");
                        }
                        sceneChanger.triggerCollision("FinalScene");
                    }
                    else
                    {
                        sceneChanger.triggerCollision(changeToScene);
                    }        
                }
                else
                    Debug.Log("SceneChanger reference is missing!");
            }
            else
                Debug.Log("PlayerController component not found on the Player!");

        }
    }

}
