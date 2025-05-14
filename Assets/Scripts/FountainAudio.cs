using UnityEngine;

public class FountainAudio : MonoBehaviour
{
    public float maxDistance = 4f; // Distance at which volume becomes 0
    private Transform player;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found! Make sure it's tagged 'Player'");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        float volume = Mathf.Clamp01(0.7f - (distance / maxDistance));
        audioSource.volume = volume;
    }
}
