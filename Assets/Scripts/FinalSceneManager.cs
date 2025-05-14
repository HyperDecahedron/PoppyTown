using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalSceneManager : MonoBehaviour
{
    public Image blackBackground;
    public Text finalSceneText;
    public GameObject EObject;
    public GameObject panelLetter;
    public Transform bloodParent;
    public Transform scoreParent;
    public Text scoreText;

    public float typingSpeed = 0.04f;

    [Header("Audio")]
    public AudioClip button_sound;
    public AudioClip scoreAudio;

    private AudioSource audioSource;

    private string[] lines = {
        "It's time I went to sleep... Oh!",
        "The Mayor is in the crops, hi!\n... ...",
    };

    private string[] lines2 = {
        "Wh-what is that light coming out of Mayor Belzy?",
        "I must be dreaming... I should go to sleep now.",
    };

    private string[] diaryLines = {
        "DIARY LINE 1",
        "DIARY OPENS",
        "DIARY LINE 2"
    };

    private string[] newspaperLine = {
        "NEWSPAPER LINES"
    };

    private string[] villagerLine = {
        "VILLAGERS LINE"
    };

    private string[] finalLine = {
        "Whereever you are, good night, Mary."
    };

    private int currentLine = 0;
    private bool isTyping = false;
    private bool lineFullyDisplayed = false;

    private PlayerController playerController;

    private enum CutsceneState
    {
        FirstSet,
        FadeOut,
        BelzyScene,
        FadeIn,
        SecondSet,
        DiaryLines,
        NewspaperLines,
        VillagerLines,
        FinalLine,
        Done
    }

    private CutsceneState currentState = CutsceneState.FirstSet;
    private string[] activeLines;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        panelLetter.SetActive(false);

        blackBackground.color = Color.black;
        finalSceneText.text = "";
        EObject.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerController.canMove = false;
        }

        activeLines = lines;
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
                audioSource.PlayOneShot(button_sound, 0.6f);
                currentLine++;

                if (currentLine < activeLines.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    ProceedToNextState();
                }
            }
        }
    }

    void ProceedToNextState()
    {
        switch (currentState)
        {
            case CutsceneState.FirstSet:
                currentState = CutsceneState.FadeOut;
                currentLine = 0;
                StartCoroutine(FadeOutBackground());
                StartCoroutine(BelzyScene());
                break;

            case CutsceneState.SecondSet:
                if (playerController.has_diary)
                {
                    currentState = CutsceneState.DiaryLines;
                    currentLine = 0;
                    activeLines = diaryLines;
                    StartCoroutine(TypeLine());
                }
                else
                {
                    currentState = CutsceneState.DiaryLines;
                    ProceedToNextState(); // Skip to next
                }
                break;

            case CutsceneState.DiaryLines:
                if (playerController.has_newspaper)
                {
                    currentState = CutsceneState.NewspaperLines;
                    currentLine = 0;
                    activeLines = newspaperLine;
                    StartCoroutine(TypeLine());
                }
                else
                {
                    currentState = CutsceneState.NewspaperLines;
                    ProceedToNextState();
                }
                break;

            case CutsceneState.NewspaperLines:
                if (playerController.has_talked)
                {
                    currentState = CutsceneState.VillagerLines;
                    currentLine = 0;
                    activeLines = villagerLine;
                    StartCoroutine(TypeLine());
                }
                else
                {
                    currentState = CutsceneState.VillagerLines;
                    ProceedToNextState();
                }
                break;

            case CutsceneState.VillagerLines:
                currentState = CutsceneState.FinalLine;
                currentLine = 0;
                activeLines = finalLine;
                StartCoroutine(TypeLine());
                break;

            case CutsceneState.FinalLine:
                currentState = CutsceneState.Done;
                EObject.SetActive(false);
                finalSceneText.gameObject.SetActive(false);
                StartCoroutine(Score());
                break;
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        lineFullyDisplayed = false;
        finalSceneText.gameObject.SetActive(true);
        finalSceneText.text = "";

        string line = activeLines[currentLine];
        foreach (char letter in line)
        {
            finalSceneText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        lineFullyDisplayed = true;
        EObject.SetActive(true);
    }

    IEnumerator FadeOutBackground()
    {
        finalSceneText.gameObject.SetActive(false);

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
    }

    IEnumerator BelzyScene()
    {
        yield return new WaitForSeconds(4f);

        if (bloodParent != null)
        {
            for (int i = 0; i < bloodParent.childCount; i++)
            {
                bloodParent.GetChild(i).gameObject.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeInBackground());
    }

    IEnumerator FadeInBackground()
    {
        finalSceneText.gameObject.SetActive(false);

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

        currentState = CutsceneState.SecondSet;
        currentLine = 0;
        activeLines = lines2;
        StartCoroutine(TypeLine());
    }

    public IEnumerator Score()
    {
        yield return new WaitForSeconds(1f); // Optional delay before starting

        scoreParent.gameObject.SetActive(true);

        int score = 0;

        for (int i = 2; i < scoreParent.childCount; i++)
        {
            Transform scoreItem = scoreParent.GetChild(i);

            bool completed = false;
            switch (i)
            {
                case 2:
                    completed = playerController.tasks >= 3;
                    break;
                case 3:
                    completed = playerController.has_diary;
                    break;
                case 4:
                    completed = playerController.has_newspaper;
                    break;
                case 5:
                    completed = playerController.has_talked_belzy;
                    break;
                case 6:
                    completed = playerController.has_talked;
                    break;
            }

            // Get the text component (assumes it's on the root or child 0)
            Text textComponent = scoreItem.GetChild(0).GetComponent<Text>();

            string originalText = textComponent != null ? textComponent.text : "";

            if (completed)
            {
                score++;
                if (textComponent != null)
                    textComponent.text = originalText;

                // Show success icon, hide failure icon
                if (scoreItem.childCount > 2)
                {
                    scoreItem.GetChild(1).gameObject.SetActive(true);  // Success
                    scoreItem.GetChild(2).gameObject.SetActive(false); // Failure
                }
            }
            else
            {
                if (textComponent != null)
                    textComponent.text = "Missing: " + originalText;

                // Hide success icon, show failure icon
                if (scoreItem.childCount > 2)
                {
                    scoreItem.GetChild(1).gameObject.SetActive(false); // Success
                    scoreItem.GetChild(2).gameObject.SetActive(true);  // Failure
                }
            }

            scoreItem.gameObject.SetActive(true);
            audioSource.PlayOneShot(scoreAudio, 0.5f);

            yield return new WaitForSeconds(1f); // Wait before showing next item
        }

        string letter_score = "F";
        if (score == 0)
        {
            letter_score = "F";
        }
        else if (score == 1)
        {
            letter_score = "E";
        }
        else if (score == 2)
        {
            letter_score = "D";
        }
        else if (score == 3)
        {
            letter_score = "C";
        }
        else if (score == 4)
        {
            letter_score = "B";
        }
        else
        {
            letter_score = "A";
        }

        scoreText.text = "SCORE: " + letter_score;
        audioSource.PlayOneShot(scoreAudio, 0.5f);
    }
}
