using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Required for UI elements like Image

public class IntroManager : MonoBehaviour
{
    public GameObject glitch;
    public GameObject postprocessing;
    public ScaleInOut panelButton;
    public Image fadeImage;  // Assign this in the inspector (a full-screen black image)

    public AudioClip glitch_short;
    public AudioClip glitch_long;
    public AudioClip creepy_short;
    private AudioSource audioSource;
    public AudioSource audioSource2;

    private bool hasStarted = false;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        postprocessing.SetActive(false);
        glitch.SetActive(false);
        if (fadeImage != null)
        {
            // Ensure the image is fully transparent at the start
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    void Update()
    {
        if (!hasStarted && Input.GetKeyDown(KeyCode.E))
        {
            hasStarted = true;
            StartCoroutine(PlayIntroSequence());
        }
    }

    private IEnumerator PlayIntroSequence()
    {
        float volume = 0.5f; 
        panelButton.stop = true;

        audioSource2.PlayOneShot(creepy_short, volume); // ost creepy box short

        glitch.SetActive(true);
        audioSource.PlayOneShot(glitch_short, volume);
        yield return new WaitForSeconds(1f);

        glitch.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        glitch.SetActive(true);
        audioSource.PlayOneShot(glitch_short, volume);
        postprocessing.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        postprocessing.SetActive(false);
        yield return new WaitForSeconds(1f);

        glitch.SetActive(false);
        audioSource.PlayOneShot(glitch_long, volume);
        postprocessing.SetActive(true);
        yield return new WaitForSeconds(4f);

        // Start fade to black
        yield return StartCoroutine(FadeToBlack());

        SceneManager.LoadScene("CutScene");
    }

    private IEnumerator FadeToBlack()
    {
        float duration = 2f; // Fade duration
        float elapsed = 0f;

        Color color = fadeImage.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = color;
            yield return null;
        }
    }
}
