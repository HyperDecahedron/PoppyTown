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

    private string[] lines = new string[]
    {
        "I received a letter a week ago.",
        "It was from my younger sister, Mary.",
        "We haven’t spoken in years, not since everything fell apart between us.",
        "And now, out of nowhere, this\nstrange message from her…",
        "What if something's really wrong?\nWhat if she’s not okay?",
        "Mary...",
        "I wish I could see you again in Poppy Town.",
    };

    private int currentLine = 0;
    private bool isTyping = false;
    private bool lineFullyDisplayed = false;

    private PlayerController playerController;

    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping) return;

            if (lineFullyDisplayed)
            {
                EObject.SetActive(false);
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
}
