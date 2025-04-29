using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class SceneChanger : MonoBehaviour
{
    public float fadeDuration = 1f;

    private Image fadeImage;
    private bool isFading = false;

    [Header("Cinemachine Virtual Camera")]
    public CinemachineVirtualCamera thisSceneCinemachine;

    // player spawning
    [Header("Previous Scenes")]
    public Transform prev_train; 
    public Transform prev_town;
    public Transform prev_mayor;
    public Transform prev_forest;
    public Transform prev_crops;
    public Transform prev_mchouse;

    private GameObject player;
    private PlayerController playerController;

    private string sceneToLoad;

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
                    if (prev_train != null)
                        player.transform.position = prev_train.position;
                    else
                        Debug.LogWarning("prev_train is null!");
                    break;

                case "PoppyTownScene":
                    if (prev_town != null)
                        player.transform.position = prev_town.position;
                    else
                        Debug.LogWarning("prev_town is null!");
                    break;

                case "MayorHouseScene":
                    if (prev_mayor != null)
                        player.transform.position = prev_mayor.position;
                    else
                        Debug.LogWarning("prev_mayor is null!");
                    break;

                case "CropsScene":
                    if (prev_crops != null)
                        player.transform.position = prev_crops.position;
                    else
                        Debug.LogWarning("prev_crops is null!");
                    break;

                case "ForestScene":
                    if (prev_forest != null)
                        player.transform.position = prev_forest.position;
                    else
                        Debug.LogWarning("prev_forest is null!");
                    break;

                case "MCHouseScene":
                    if (prev_mchouse != null)
                        player.transform.position = prev_mchouse.position;
                    else
                        Debug.LogWarning("prev_mchouse is null!");
                    break;

                default:
                    break;
            }

            // put the player as the follow object of the cinemachine camera of this scene (set in the variable above)
            if (thisSceneCinemachine != null)
            {
                thisSceneCinemachine.Follow = player.transform;
                Debug.Log("Cinemachine follow set");
            }
            else
                Debug.Log("Cinemachine Camera not assigned in SceneChanger!");

            // Update sound of the steps if inside interior
            string this_scene = SceneManager.GetActiveScene().name;

            if (this_scene == "MCHouseScene" || this_scene == "MayorHouseScene")
            {
                playerController.isInterior = true;
            }
            else
            {
                playerController.isInterior = false;
            }
        }
    }

    public void triggerCollision(string new_scene)
    {
        if (!isFading)
        {
            sceneToLoad = new_scene;
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
