using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiaryIconManager : MonoBehaviour
{
    public bool canOpen = false; 

    [SerializeField] private Transform transformOut;
    [SerializeField] private Transform transformIn;
    [SerializeField] private float secondsAnimation = 1f;
    [SerializeField] private AudioClip soundOpen;
    [SerializeField] private AudioClip soundClose;
    [SerializeField] private AudioClip soundTab;
    [SerializeField] private GameObject diaryObject;

    [SerializeField] private GameObject page1;
    [SerializeField] private GameObject page2;
    [SerializeField] private GameObject page3;

    [Header("Page 1 Texts")]
    [SerializeField] private Text page1_text1;
    [SerializeField] private Text page1_text2;
    [SerializeField] private Text page1_text3;

    private string text1; 
    private string text2;
    private string text3;

    private bool isDiaryOpen = false;
    private bool isAnimating = false;
    private bool hasOpenedDiaryBefore = false;

    private AudioSource audioSource;
    private PlayerController playerController; 
    private int currentPage = 1;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        diaryObject.transform.position = transformOut.position;
        ShowPage(currentPage);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        // Clear texts initially
        text1 = page1_text1.text;
        text2 = page1_text2.text;
        text3 = page1_text3.text;
        page1_text1.text = "";
        page1_text2.text = "";
        page1_text3.text = "";
    }

    private void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.Q) && !isAnimating)
        {
            if (isDiaryOpen) // close diary
            {
                playerController.canMove = true; 
                StartCoroutine(MoveDiary(transformIn.position, transformOut.position, soundClose));
                currentPage = 1;
            }
            else // open diary
            {
                playerController.canMove = false;
                ShowPage(currentPage);
                StartCoroutine(MoveDiary(transformOut.position, transformIn.position, soundOpen));
            }

            isDiaryOpen = !isDiaryOpen;
        }

        if (isDiaryOpen && !isAnimating)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                currentPage = currentPage == 3 ? 1 : currentPage + 1;
                ShowPage(currentPage);
                audioSource.PlayOneShot(soundTab);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                currentPage = currentPage == 1 ? 3 : currentPage - 1;
                ShowPage(currentPage);
                audioSource.PlayOneShot(soundTab);
            }
        }
    }

    private System.Collections.IEnumerator MoveDiary(Vector3 from, Vector3 to, AudioClip clip)
    {
        isAnimating = true;
        float elapsed = 0f;
        audioSource.PlayOneShot(clip);

        while (elapsed < secondsAnimation)
        {
            diaryObject.transform.position = Vector3.Lerp(from, to, elapsed / secondsAnimation);
            elapsed += Time.deltaTime;
            yield return null;
        }

        diaryObject.transform.position = to;
        isAnimating = false;

        if (!hasOpenedDiaryBefore)
        {
            hasOpenedDiaryBefore = true;
            // Start the typewriter effect for text1, text2, and text3
            StartCoroutine(TypeWriterEffect(page1_text1, text1, 0.04f, () =>
            {
                StartCoroutine(TypeWriterEffect(page1_text2, text2, 0.04f, () =>
                {
                    StartCoroutine(TypeWriterEffect(page1_text3, text3, 0.04f, () =>
                    {
                        // All three texts finished typing, call to cutscene manager
                        GameObject cutScene = GameObject.FindGameObjectWithTag("CutSceneManager");
                        if (cutScene != null)
                        {
                            CutSceneManager cutSceneManager = cutScene.GetComponent<CutSceneManager>();
                            cutSceneManager.FinishCutScene();
                        }
                    }));
                }));
            }));
        }
    }

    private System.Collections.IEnumerator TypeWriterEffect(Text targetText, string targetString, float delay, System.Action onComplete)
    {
        targetText.text = "";  // Clear text initially
        foreach (char letter in targetString.ToCharArray())
        {
            targetText.text += letter;  // Add letter one by one
            yield return new WaitForSeconds(delay);  // Wait for the specified delay
        }

        yield return new WaitForSeconds(1f);

        // If there's a callback, execute it after the typewriter effect finishes
        onComplete?.Invoke();
    }

    private void ShowPage(int page)
    {
        page1.SetActive(page == 1);
        page2.SetActive(page == 2);
        page3.SetActive(page == 3);
    }

    public void ResetDiaryPosition()
    {
        diaryObject.transform.position = transformOut.position;
        isDiaryOpen = false;
        playerController.canMove = true;
        currentPage = 1;
        canOpen = true;
    }
}
