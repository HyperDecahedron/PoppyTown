using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiaryIconManager : MonoBehaviour
{
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

    private bool isDiaryOpen = false;
    private bool isAnimating = false;
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isAnimating)
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
    }

    private void ShowPage(int page)
    {
        page1.SetActive(page == 1);
        page2.SetActive(page == 2);
        page3.SetActive(page == 3);
    }
}
