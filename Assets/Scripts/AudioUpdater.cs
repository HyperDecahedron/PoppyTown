using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUpdater : MonoBehaviour
{
    public AudioClip normal_sound;
    public AudioClip creepy_sound;
    public float volume_normal = 0.5f;
    public float volume_creepy = 0.5f;

    private AudioSource audioSource;
    private PlayerController playerController;

    void Start()
    {
        // Get the AudioSource on this GameObject
        audioSource = GetComponent<AudioSource>();

        // Find the GameObject with the "Player" tag and get its PlayerController
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found!");
        }

        UpdateAudio();
    }

    void UpdateAudio()
    {
        if (playerController == null || audioSource == null) return;

        if (playerController.state >= 5 || playerController.startCreepy)
        {
            if (audioSource.clip != creepy_sound)
            {
                audioSource.clip = creepy_sound;
                audioSource.volume = volume_creepy; 
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip != normal_sound)
            {
                audioSource.clip = normal_sound;
                audioSource.volume = volume_normal;
                audioSource.Play();
            }
        }
    }

}
