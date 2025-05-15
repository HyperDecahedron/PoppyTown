using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    public Image blackBackground;
    public Image upperBackground;
    public Text cutSceneText;
    public GameObject EObject;
    public GameObject QObject;
    public DiaryIconManager diaryManager;
    public GameObject panelLetter;

    // to set active before changing the scene
    public GameObject TimeUI;

    public float typingSpeed = 0.04f;

    [Header("Dialog objects")]
    public GameObject polina_panel;
    public Text polina_text;
    public GameObject belzy_panel;
    public Text belzy_text1;
    public Text belzy_text2;

    private string p_text;
    private string b_text1; 
    private string b_text2;

    [Header("Audio")]
    public AudioClip button_sound;
    public AudioClip polina_sound;
    public AudioClip belzy_sound;
    private AudioSource audioSource;

    private string[] lines = new string[]
    {
        "I received a letter a week ago.",
        "It was from my younger sister, Mary.",
        "  ",
        "We haven’t spoken in years, not since everything fell apart between us.",
        "And now, out of nowhere,\nthis strange letter…",
        "This is unlike her...\nWhat if she’s not okay?",
        "Mary...",
        "Tomorrow I'll embark on a journey to Poppy Town.\nI wish to see you again.",
    };

    private int currentLine = 0;
    private bool isTyping = false;
    private bool lineFullyDisplayed = false;

    private PlayerController playerController;

    private bool canPressE = true;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        panelLetter.SetActive(false);

        // Set black background at start
        blackBackground.color = Color.black;
        cutSceneText.text = "";
        EObject.SetActive(false);
        TimeUI.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerController.canMove = false; 
        }

        // dialog settings
        polina_panel.SetActive(false);
        belzy_panel.SetActive(false);

        b_text1 = belzy_text1.text;
        b_text2 = belzy_text2.text;
        p_text = polina_text.text;

        StartCoroutine(TypeLine());      
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPressE)
        {
            if (isTyping) return;

            if (lineFullyDisplayed)
            {
                if (currentLine == 1)
                {
                    StartCoroutine(ResizeLetterPanel());
                }
                else if (currentLine == 2)
                {
                    panelLetter.SetActive(false);
                }

                EObject.SetActive(false);
                audioSource.PlayOneShot(button_sound, 0.6f);
                currentLine++;

                if (currentLine < lines.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    // All lines finished, fade background
                    StartCoroutine(FadeOutBackground());

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (QObject.activeSelf)
            {
                audioSource.PlayOneShot(button_sound);
            }
            QObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            FinishCutScene();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        lineFullyDisplayed = false;
        cutSceneText.text = "";

        string line = lines[currentLine];
        foreach (char letter in line)
        {
            cutSceneText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        lineFullyDisplayed = true;
        EObject.SetActive(true);
    }

    IEnumerator FadeOutBackground()
    {
        cutSceneText.gameObject.SetActive(false);

        float duration = 2f;
        float elapsed = 0f;
        Color color = blackBackground.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / duration);
            blackBackground.color = color;
            yield return null;
        }

        // fake player movement to the right
        canPressE = false;
        playerController.FakeRightMovement();
    }

    public void TriggerDetected()
    {
        // start Dialog corroutine
        StartCoroutine(DialogCoroutine());
    }

    IEnumerator DialogCoroutine()
    {
        // stop fake movement
        playerController.StopFakeMovement();
        yield return new WaitForSeconds(1f);

        polina_text.gameObject.SetActive(true);
        belzy_text1.gameObject.SetActive(true);
        belzy_text2.gameObject.SetActive(true);
        belzy_text1.text = "";
        belzy_text2.text = "";
        polina_text.text = "";

        // dialog polina
        polina_panel.SetActive(true);
        audioSource.PlayOneShot(polina_sound, 0.2f);

        string line = p_text;
        foreach (char letter in line)
        {
            polina_text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(2.5f);
        polina_panel.SetActive(false);

        // dialog belzy
        belzy_panel.SetActive(true);
        audioSource.PlayOneShot(belzy_sound, 0.3f);

        line = b_text1;
        foreach (char letter in line)
        {
            belzy_text1.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(2.5f);
        belzy_text1.text = "";

        line = b_text2;
        foreach (char letter in line)
        {
            belzy_text2.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(2.5f);
        
        belzy_panel.SetActive(false);

        // fade in cutscene
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // black background appears
        float duration = 2f;
        float elapsed = 0f;
        Color color = blackBackground.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            blackBackground.color = color;
            yield return null;
        }

        // Now, the diary part
        QObject.SetActive(true);
        diaryManager.canOpen = true;
    }

    public void FinishCutScene()
    {
        // Fade out to black 
        StartCoroutine(FadeToBlackAtTheEnd());
    }

    IEnumerator FadeToBlackAtTheEnd()
    {
        float duration = 1f;
        float elapsed = 0f;
        Color color = upperBackground.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / duration);
            upperBackground.color = color;
            yield return null;
        }

        // now move the diary out
        diaryManager.ResetDiaryPosition();
        TimeUI.SetActive(true);
        playerController.canMove = true;
        playerController.isFakeMoving = false;

        playerController.prevScene = "CutScene";

        SceneManager.LoadScene("MCHouseScene");
    }


    IEnumerator ResizeLetterPanel()
    {
        panelLetter.SetActive(true);

        // Set initial scale to Y = 0
        Vector3 initialScale = new Vector3(panelLetter.transform.localScale.x, 0f, panelLetter.transform.localScale.z);
        // Target scale is Y = 1
        Vector3 targetScale = new Vector3(panelLetter.transform.localScale.x, 1f, panelLetter.transform.localScale.z);

        // Resize panelLetter from Y = 0 to Y = 1 in 0.5s
        float timeElapsed = 0f;
        while (timeElapsed < 0.5f)
        {
            timeElapsed += Time.deltaTime;
            panelLetter.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / 0.5f);
            yield return null;
        }

        panelLetter.transform.localScale = targetScale;

        // Re-enable the E button to continue
        EObject.SetActive(true);
    }


}
