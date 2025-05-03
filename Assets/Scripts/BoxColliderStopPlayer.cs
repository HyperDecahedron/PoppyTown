using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderStopPlayer : MonoBehaviour
{
    public CutSceneManager cutSceneManager; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cutSceneManager.TriggerDetected();
        }
    }
}
