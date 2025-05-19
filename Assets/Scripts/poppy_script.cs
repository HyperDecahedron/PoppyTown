using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poppy_script : MonoBehaviour
{
    private PlayerController playerController;
    public bool setActiveAfter5 = true;

    // Start is called before the first frame update
    void Start()
    {
        // Get player controller
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();

            if (setActiveAfter5)
            {
                if (playerController.state > 5 || playerController.startCreepy)
                {
                    this.gameObject.SetActive(true);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                if (playerController.state > 5 || playerController.startCreepy)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                    this.gameObject.SetActive(true);
                }
            }

            

        }
        else
        {
            Debug.Log("Character: PlayerController not found");
        }
    }

}
