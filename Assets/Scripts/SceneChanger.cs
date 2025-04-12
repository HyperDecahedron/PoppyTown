using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad = "PoppyTownScene";
    public float fadeDuration = 1f;

    private Image fadeImage;
    private bool isFading = false;

    // player spawning
    public Transform spawnInit; // init from that scene
    public Transform spawnExit; // exit from that scene

    private GameObject player;
    private PlayerController playerController;

    private void Start()
    {
        // Find the black image inside this object
        fadeImage = GetComponentInChildren<Image>();

        if (fadeImage != null)
        {
            // Start fully black
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;

            // Fade in
            StartCoroutine(FadeIn());

            // Spawn player in the correct position
            player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();

            Debug.Log("Spawning character according to prev scene: " + playerController.prevScene);

            switch (playerController.prevScene)
            {
                case "TrainStationScene":
                    player.transform.position = spawnInit.position;
                    break;
                case "PoppyTownScene":
                    player.transform.position = spawnExit.position;
                    break;
                default:
                    player.transform.position = spawnInit.position;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFading && collision.CompareTag("Player"))
        {
            // Get the current scene name
            string currentScene = SceneManager.GetActiveScene().name;

            // Set the variable prevScene inside the player
            playerController.prevScene = currentScene;
            Debug.Log("updated prev scene to: " + currentScene);

            StartCoroutine(FadeAndChangeScene());
        }
    }

    private IEnumerator FadeIn()
    {
        isFading = true;
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
        isFading = false;
    }

    private IEnumerator FadeAndChangeScene()
    {
        isFading = true;

        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
